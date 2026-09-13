import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ClientService } from '../../../core/services/client.service';
import { ToastService } from '../../../core/services/toast.service';
import { Client } from '../../../core/models/client.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './client-list.html',
  styleUrl: './client-list.scss',
})
export class ClientList implements OnInit {
  private clientService = inject(ClientService);
  private toastService = inject(ToastService);

  clients = signal<Client[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    this.loadClients();
  }

  totalClients = computed(() => this.clients().length);

  nouveauxCeMois = computed(() => {
    const now = new Date();
    return this.clients().filter(c => {
      const d = new Date(c.dateCreation);
      return d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear();
    }).length;
  });


  loadClients(): void {
    this.loading.set(true);
    this.clientService.getAll().subscribe({
      next: (data) => {
        this.clients.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.toastService.error('Impossible de charger les clients.');
        this.loading.set(false);
      },
    });
  }

  confirmDelete(client: Client): void {
    const confirmed = window.confirm(
      `Supprimer le client "${client.nom} ${client.prenomOuRaisonSociale}" ?`,
    );
    if (!confirmed) return;

    this.clientService.delete(client.id).subscribe({
      next: () => {
        this.toastService.success('Client supprimé.');
        this.loadClients();
      },
      error: () => {
        this.toastService.error(
          'Suppression impossible (le client a peut-être des commandes liées).',
        );
      },
    });
  }
}
