import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { ToastService } from '../../../core/services/toast.service';
import { Order } from '../../../core/models/order.model';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './order-list.html',
  styleUrl: './order-list.scss',
})
export class OrderList implements OnInit {
  private orderService = inject(OrderService);
  private toastService = inject(ToastService);

  orders = signal<Order[]>([]);
  loading = signal(true);

  totalCommandes = computed(() => this.orders().length);

  enBrouillon = computed(() => this.orders().filter((o) => o.statut === 'Brouillon').length);

  chiffreAffaires = computed(() =>
    this.orders()
      .filter((o) => o.statut === 'Validee')
      .reduce((sum, o) => sum + o.totalTTC, 0),
  );

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading.set(true);
    this.orderService.getAll().subscribe({
      next: (data) => {
        this.orders.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.toastService.error('Impossible de charger les commandes.');
        this.loading.set(false);
      },
    });
  }

  confirmDelete(order: Order): void {
    const confirmed = window.confirm(`Supprimer la commande "${order.numeroCommande}" ?`);
    if (!confirmed) return;

    this.orderService.delete(order.id).subscribe({
      next: () => {
        this.toastService.success('Commande supprimée.');
        this.loadOrders();
      },
      error: () => {
        this.toastService.error('Suppression impossible.');
      },
    });
  }

  // Convertit le statut en classe CSS pour le badge (voir styles.scss)
  statusClass(order: Order): string {
    if (order.statut === 'Validee') return 'status-validee';
    if (order.statut === 'Annulee') return 'status-annulee';
    return 'status-brouillon';
  }

  statusLabel(order: Order): string {
    if (order.statut === 'Validee') return 'Validée';
    if (order.statut === 'Annulee') return 'Annulée';
    return 'Brouillon';
  }
}
