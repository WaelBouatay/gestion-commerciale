import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../../../core/services/client.service';
import { ToastService } from '../../../core/services/toast.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-client-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './client-form.html',
  styleUrl: './client-form.scss',
})
export class ClientForm implements OnInit {
  private fb = inject(FormBuilder);
  private clientService = inject(ClientService);
  private toastService = inject(ToastService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  clientId = signal<number | null>(null);
  saving = signal(false);

  // Le formulaire réactif : chaque champ a une valeur initiale + des règles de validation
  form = this.fb.group({
    nom: ['', [Validators.required, Validators.maxLength(100)]],
    prenomOuRaisonSociale: ['', [Validators.required, Validators.maxLength(150)]],
    email: ['', [Validators.required, Validators.email]],
    telephone: ['', [Validators.required]],
    adressesText: [''], // on saisit les adresses sous forme de texte, une par ligne
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.clientId.set(id);
      this.clientService.getById(id).subscribe({
        next: (client) => {
          this.form.patchValue({
            nom: client.nom,
            prenomOuRaisonSociale: client.prenomOuRaisonSociale,
            email: client.email,
            telephone: client.telephone,
            adressesText: client.adresses.join('\n'),
          });
        },
        error: () => this.toastService.error('Client introuvable.'),
      });
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
      nom: raw.nom!,
      prenomOuRaisonSociale: raw.prenomOuRaisonSociale!,
      email: raw.email!,
      telephone: raw.telephone!,
      adresses: (raw.adressesText ?? '')
        .split('\n')
        .map((a) => a.trim())
        .filter((a) => a.length > 0),
    };

    const id = this.clientId();

    if (id) {
      this.clientService.update(id, payload).subscribe({
        next: () => {
          this.toastService.success('Client modifié.');
          this.router.navigate(['/clients']);
        },
        error: () => {
          this.toastService.error('Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    } else {
      this.clientService.create(payload).subscribe({
        next: () => {
          this.toastService.success('Client créé.');
          this.router.navigate(['/clients']);
        },
        error: () => {
          this.toastService.error('Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    }
  }
}

