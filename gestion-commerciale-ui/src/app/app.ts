import { Component } from '@angular/core';
import { Shell } from './shared/components/shell/shell';
import { ToastContainer } from './shared/components/toast-container/toast-container';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Shell, ToastContainer],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {}
