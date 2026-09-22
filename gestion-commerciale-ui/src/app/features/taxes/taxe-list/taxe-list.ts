import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { TaxeService } from '../../../core/services/taxe.service';
import { ToastService } from '../../../core/services/toast.service';
import { Taxe } from '../../../core/models/taxe.model';

@Component({
  selector: 'app-taxe-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './taxe-list.html',
  styleUrl: './taxe-list.scss',
})
export class TaxeList implements OnInit {
  private taxeService = inject(TaxeService);
  private toastService = inject(ToastService);

  taxes = signal<Taxe[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    this.loadTaxes();
  }

  loadTaxes(): void {
    this.loading.set(true);
    this.taxeService.getAll().subscribe({
      next: (data) => {
        this.taxes.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.toastService.error('Impossible de charger les taxes.');
        this.loading.set(false);
      },
    });
  }

  confirmDelete(taxe: Taxe): void {
    const confirmed = window.confirm(`Supprimer la taxe "${taxe.libelle}" ?`);
    if (!confirmed) return;

    this.taxeService.delete(taxe.id).subscribe({
      next: () => {
        this.toastService.success('Taxe supprimée.');
        this.loadTaxes();
      },
      error: () => this.toastService.error('Suppression impossible.'),
    });
  }

  valeurAffichee(taxe: Taxe): string {
    return taxe.type === 'Pourcentage' ? `${taxe.valeur}%` : `${taxe.valeur.toFixed(2)} DT`;
  }
}
