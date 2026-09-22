import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { RemiseService } from '../../../core/services/remise.service';
import { ToastService } from '../../../core/services/toast.service';
import { TypeRemise } from '../../../core/models/remise.model';

@Component({
  selector: 'app-remise-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './remise-form.html',
  styleUrl: './remise-form.scss',
})
export class RemiseForm implements OnInit {
  private fb = inject(FormBuilder);
  private remiseService = inject(RemiseService);
  private toastService = inject(ToastService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  remiseId = signal<number | null>(null);
  saving = signal(false);

  form = this.fb.group({
    libelle: ['', [Validators.required, Validators.maxLength(100)]],
    type: ['Pourcentage' as TypeRemise, [Validators.required]],
    valeur: [0, [Validators.required, Validators.min(0)]],
    active: [true],
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.remiseId.set(Number(idParam));
      // Remarque : pas de GET /api/remises/{id} côté backend pour l'instant.
      // Pour une vraie version, ajouter RemiseService.getById() + endpoint associé.
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

    const id = this.remiseId();

    if (id) {
      this.remiseService.update(id, payload).subscribe({
        next: () => {
          this.toastService.success('Remise modifiée.');
          this.router.navigate(['/remises']);
        },
        error: () => {
          this.toastService.error('Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    } else {
      this.remiseService.create(payload).subscribe({
        next: () => {
          this.toastService.success('Remise créée.');
          this.router.navigate(['/remises']);
        },
        error: () => {
          this.toastService.error('Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    }
  }
}
