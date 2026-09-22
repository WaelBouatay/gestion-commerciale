import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { RemiseService } from '../../../core/services/remise.service';
import { ToastService } from '../../../core/services/toast.service';
import { Remise } from '../../../core/models/remise.model';

@Component({
  selector: 'app-remise-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './remise-list.html',
  styleUrl: './remise-list.scss',
})
export class RemiseList implements OnInit {
  private remiseService = inject(RemiseService);
  private toastService = inject(ToastService);

  remises = signal<Remise[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    this.loadRemises();
  }

  loadRemises(): void {
    this.loading.set(true);
    this.remiseService.getAll().subscribe({
      next: (data) => {
        this.remises.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.toastService.error('Impossible de charger les remises.');
        this.loading.set(false);
      },
    });
  }

  confirmDelete(remise: Remise): void {
    const confirmed = window.confirm(`Supprimer la remise "${remise.libelle}" ?`);
    if (!confirmed) return;

    this.remiseService.delete(remise.id).subscribe({
      next: () => {
        this.toastService.success('Remise supprimée.');
        this.loadRemises();
      },
      error: () => this.toastService.error('Suppression impossible.'),
    });
  }

  valeurAffichee(remise: Remise): string {
    return remise.type === 'Pourcentage' ? `${remise.valeur}%` : `${remise.valeur.toFixed(2)} DT`;
  }
}
