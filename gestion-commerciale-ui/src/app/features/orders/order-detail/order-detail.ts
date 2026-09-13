import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { ToastService } from '../../../core/services/toast.service';
import { Order } from '../../../core/models/order.model';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.scss',
})
export class OrderDetail implements OnInit {
  private orderService = inject(OrderService);
  private toastService = inject(ToastService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  order = signal<Order | null>(null);
  loading = signal(true);
  validating = signal(false);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadOrder(id);
  }

  loadOrder(id: number): void {
    this.loading.set(true);
    this.orderService.getById(id).subscribe({
      next: (data) => {
        this.order.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.toastService.error('Commande introuvable.');
        this.loading.set(false);
      },
    });
  }

  validate(): void {
    const current = this.order();
    if (!current) return;

    this.validating.set(true);
    this.orderService.validate(current.id).subscribe({
      next: (updated) => {
        this.order.set(updated);
        this.toastService.success('Commande validée. Le stock a été mis à jour.');
        this.validating.set(false);
      },
      error: (err) => {
        this.toastService.error(err.error?.message ?? 'Validation impossible.');
        this.validating.set(false);
      },
    });
  }

  statusClass(): string {
    const statut = this.order()?.statut;
    if (statut === 'Validee') return 'status-validee';
    if (statut === 'Annulee') return 'status-annulee';
    return 'status-brouillon';
  }

  statusLabel(): string {
    const statut = this.order()?.statut;
    if (statut === 'Validee') return 'Validée';
    if (statut === 'Annulee') return 'Annulée';
    return 'Brouillon';
  }
}
