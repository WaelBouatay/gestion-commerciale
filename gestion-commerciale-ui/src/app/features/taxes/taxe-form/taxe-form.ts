import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TaxeService } from '../../../core/services/taxe.service';
import { ToastService } from '../../../core/services/toast.service';
import { TypeTaxe } from '../../../core/models/taxe.model';

@Component({
  selector: 'app-taxe-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './taxe-form.html',
  styleUrl: './taxe-form.scss',
})
export class TaxeForm implements OnInit {
  private fb = inject(FormBuilder);
  private taxeService = inject(TaxeService);
  private toastService = inject(ToastService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  taxeId = signal<number | null>(null);
  saving = signal(false);

  form = this.fb.group({
    libelle: ['', [Validators.required, Validators.maxLength(100)]],
    type: ['Pourcentage' as TypeTaxe, [Validators.required]],
    valeur: [0, [Validators.required, Validators.min(0)]],
    active: [true],
  });

  ngOnInit(): void {
    // Pas de GetById côté API pour l'instant (on n'en a pas eu besoin ailleurs) —
    // pour la modification, on récupère la taxe depuis la liste déjà chargée en amont.
    // Alternative simple ici : ajouter un GetByIdAsync si besoin réel de rechargement direct.
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.taxeId.set(Number(idParam));
      // Optionnel : si tu veux un vrai rechargement, ajoute GetById à TaxeService/TaxesController
      // comme fait pour Client/Product/Order.
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const raw = this.form.getRawValue();

    const payload = {
      libelle: raw.libelle!,
      type: raw.type!,
      valeur: raw.valeur!,
      active: raw.active!,
    };

    const id = this.taxeId();

    if (id) {
      this.taxeService.update(id, payload).subscribe({
        next: () => {
          this.toastService.success('Taxe modifiée.');
          this.router.navigate(['/taxes']);
        },
        error: () => {
          this.toastService.error('Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    } else {
      this.taxeService.create(payload).subscribe({
        next: () => {
          this.toastService.success('Taxe créée.');
          this.router.navigate(['/taxes']);
        },
        error: () => {
          this.toastService.error('Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    }
  }
}
