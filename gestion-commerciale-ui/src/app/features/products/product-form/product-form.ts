import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './product-form.html',
  styleUrl: './product-form.scss',
})
export class ProductForm implements OnInit {
  private fb = inject(FormBuilder);
  private productService = inject(ProductService);
  private toastService = inject(ToastService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  productId = signal<number | null>(null);
  saving = signal(false);

  form = this.fb.group({
    reference: ['', [Validators.required]],
    nom: ['', [Validators.required, Validators.maxLength(150)]],
    description: [''],
    prixUnitaireHT: [0, [Validators.required, Validators.min(0.01)]],
    quantiteEnStock: [0, [Validators.required, Validators.min(0)]],
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.productId.set(id);
      this.productService.getById(id).subscribe({
        next: (product) => {
          this.form.patchValue({
            reference: product.reference,
            nom: product.nom,
            description: product.description,
            prixUnitaireHT: product.prixUnitaireHT,
            quantiteEnStock: product.quantiteEnStock,
          });
        },
        error: () => this.toastService.error('Produit introuvable.'),
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
      reference: raw.reference!,
      nom: raw.nom!,
      description: raw.description ?? '',
      prixUnitaireHT: raw.prixUnitaireHT!,
      quantiteEnStock: raw.quantiteEnStock!,
    };

    const id = this.productId();

    if (id) {
      this.productService.update(id, payload).subscribe({
        next: () => {
          this.toastService.success('Produit modifié.');
          this.router.navigate(['/produits']);
        },
        error: () => {
          this.toastService.error('Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    } else {
      this.productService.create(payload).subscribe({
        next: () => {
          this.toastService.success('Produit créé.');
          this.router.navigate(['/produits']);
        },
        error: () => {
          this.toastService.error('Une erreur est survenue.');
          this.saving.set(false);
        },
      });
    }
  }
}
