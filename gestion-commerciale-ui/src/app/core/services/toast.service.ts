import { Injectable, signal } from '@angular/core';

export interface Toast {
  id: number;
  message: string;
  type: 'success' | 'error';
}

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private nextId = 0;

  // "signal" est la nouvelle façon (Angular 17+) de gérer un état réactif simple
  toasts = signal<Toast[]>([]);

  success(message: string): void {
    this.show(message, 'success');
  }

  error(message: string): void {
    this.show(message, 'error');
  }

  private show(message: string, type: 'success' | 'error'): void {
    const id = this.nextId++;
    this.toasts.update((list) => [...list, { id, message, type }]);

    // Le toast disparaît tout seul après 4 secondes
    setTimeout(() => {
      this.toasts.update((list) => list.filter((t) => t.id !== id));
    }, 4000);
  }
}
