import { Routes } from '@angular/router';
import { ClientList } from './features/clients/client-list/client-list';
import { ClientForm } from './features/clients/client-form/client-form';
import { ProductList } from './features/products/product-list/product-list';
import { ProductForm } from './features/products/product-form/product-form';
import { OrderList } from './features/orders/order-list/order-list';
import { OrderForm } from './features/orders/order-form/order-form';
import { OrderDetail } from './features/orders/order-detail/order-detail';
import { TaxeList } from './features/taxes/taxe-list/taxe-list';
import { TaxeForm } from './features/taxes/taxe-form/taxe-form';

import { RemiseList } from './features/remises/remise-list/remise-list';
import { RemiseForm } from './features/remises/remise-form/remise-form';


export const routes: Routes = [
  { path: '', redirectTo: 'clients', pathMatch: 'full' },

  { path: 'clients', component: ClientList },
  { path: 'clients/nouveau', component: ClientForm },
  { path: 'clients/:id/modifier', component: ClientForm },

  { path: 'produits', component: ProductList },
  { path: 'produits/nouveau', component: ProductForm },
  { path: 'produits/:id/modifier', component: ProductForm },

  { path: 'commandes', component: OrderList },
  { path: 'commandes/nouvelle', component: OrderForm },
  { path: 'commandes/:id/modifier', component: OrderForm },
  { path: 'commandes/:id', component: OrderDetail },

  { path: 'taxes', component: TaxeList },
  { path: 'taxes/nouvelle', component: TaxeForm },
  { path: 'taxes/:id/modifier', component: TaxeForm },

  { path: 'remises', component: RemiseList },
  { path: 'remises/nouvelle', component: RemiseForm },
  { path: 'remises/:id/modifier', component: RemiseForm },
];
