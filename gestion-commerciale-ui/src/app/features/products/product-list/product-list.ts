import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { ToastService } from '../../../core/services/toast.service';
import { Product } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss',
})
export class ProductList implements OnInit {
  private productService = inject(ProductService);
  private toastService = inject(ToastService);

  products = signal<Product[]>([]);
  loading = signal(true);

  totalProduits = computed(() => this.products().length);

  valeurStock = computed(() =>
    this.products().reduce((sum, p) => sum + p.prixUnitaireHT * p.quantiteEnStock, 0),
  );

  stockFaibleCount = computed(() => this.products().filter((p) => this.isLowStock(p)).length);

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading.set(true);
    this.productService.getAll().subscribe({
      next: (data) => {
        this.products.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.toastService.error('Impossible de charger les produits.');
        this.loading.set(false);
      },
    });
  }

  confirmDelete(product: Product): void {
    const confirmed = window.confirm(`Supprimer le produit "${product.nom}" ?`);
    if (!confirmed) return;

    this.productService.delete(product.id).subscribe({
      next: () => {
        this.toastService.success('Produit supprimé.');
        this.loadProducts();
      },
      error: () => {
        this.toastService.error('Suppression impossible (le produit a peut-être été commandé).');
      },
    });
  }

  // Petit indicateur visuel : stock bas
  isLowStock(product: Product): boolean {
    return product.quantiteEnStock <= 5;
  }
}
