import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../../../core/services/client.service';
import { ProductService } from '../../../core/services/product.service';
import { OrderService } from '../../../core/services/order.service';
import { ToastService } from '../../../core/services/toast.service';
import { Client } from '../../../core/models/client.model';
import { Product } from '../../../core/models/product.model';

// Une ligne "en cours de saisie" dans le formulaire
interface DraftLine {
  productId: number | null;
  quantite: number;
}

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './order-form.html',
  styleUrl: './order-form.scss',
})
export class OrderForm implements OnInit {
  private clientService = inject(ClientService);
  private productService = inject(ProductService);
  private orderService = inject(OrderService);
  private toastService = inject(ToastService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  orderId = signal<number | null>(null);
  saving = signal(false);

  clients = signal<Client[]>([]);
  products = signal<Product[]>([]);

  selectedClientId = signal<number | null>(null);
  lines = signal<DraftLine[]>([{ productId: null, quantite: 1 }]);

  // Total HT recalculé automatiquement à chaque changement de "lines" ou "products"
  // (aperçu uniquement — le vrai calcul, source de vérité, se fait toujours côté serveur)
  totalHTPreview = computed(() => {
    return this.lines().reduce((sum, line) => {
      const product = this.products().find((p) => p.id === line.productId);
      if (!product) return sum;
      return sum + product.prixUnitaireHT * line.quantite;
    }, 0);
  });

  ngOnInit(): void {
    this.clientService.getAll().subscribe({
      next: (data) => this.clients.set(data),
      error: () => this.toastService.error('Impossible de charger les clients.'),
    });

    this.productService.getAll().subscribe({
      next: (data) => this.products.set(data),
      error: () => this.toastService.error('Impossible de charger les produits.'),
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.orderId.set(id);
      this.orderService.getById(id).subscribe({
        next: (order) => {
          this.selectedClientId.set(order.clientId);
          this.lines.set(
            order.lignes.map((l) => ({ productId: l.productId, quantite: l.quantite })),
          );
        },
        error: () => this.toastService.error('Commande introuvable.'),
      });
    }
  }

  productPrice(productId: number | null): number {
    const product = this.products().find((p) => p.id === productId);
    return product ? product.prixUnitaireHT : 0;
  }

  productStock(productId: number | null): number {
    const product = this.products().find((p) => p.id === productId);
    return product ? product.quantiteEnStock : 0;
  }

  addLine(): void {
    this.lines.update((list) => [...list, { productId: null, quantite: 1 }]);
  }

  removeLine(index: number): void {
    this.lines.update((list) => list.filter((_, i) => i !== index));
  }

  onProductChange(index: number, productId: string): void {
    this.lines.update((list) => {
      const copy = [...list];
      copy[index] = { ...copy[index], productId: productId ? Number(productId) : null };
      return copy;
    });
  }

  onQuantiteChange(index: number, quantite: string): void {
    this.lines.update((list) => {
      const copy = [...list];
      copy[index] = { ...copy[index], quantite: Number(quantite) || 0 };
      return copy;
    });
  }

  canSubmit(): boolean {
    if (!this.selectedClientId()) return false;
    if (this.lines().length === 0) return false;
    return this.lines().every((l) => l.productId !== null && l.quantite > 0);
  }

  submit(): void {
    if (!this.canSubmit()) {
      this.toastService.error('Sélectionnez un client et au moins une ligne valide.');
      return;
    }

    this.saving.set(true);

    const payload = {
      clientId: this.selectedClientId()!,
      lignes: this.lines().map((l) => ({ productId: l.productId!, quantite: l.quantite })),
    };

    const id = this.orderId();

    if (id) {
      this.orderService.update(id, payload).subscribe({
        next: () => {
          this.toastService.success('Commande modifiée.');
          this.router.navigate(['/commandes', id]);
        },
        error: (err) => {
          this.toastService.error(err.error?.message ?? 'Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    } else {
      this.orderService.create(payload).subscribe({
        next: (created) => {
          this.toastService.success('Commande créée.');
          this.router.navigate(['/commandes', created.id]);
        },
        error: (err) => {
          this.toastService.error(err.error?.message ?? 'Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    }
  }
}
