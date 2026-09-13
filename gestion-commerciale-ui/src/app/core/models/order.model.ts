export type OrderStatus = 'Brouillon' | 'Validee' | 'Annulee';

export interface OrderLine {
  id: number;
  productId: number;
  productNom: string;
  quantite: number;
  prixUnitaire: number;
  totalLigne: number;
}

export interface Order {
  id: number;
  numeroCommande: string;
  clientId: number;
  clientNom: string;
  dateCommande: string;
  statut: OrderStatus;
  totalHT: number;
  totalTTC: number;
  lignes: OrderLine[];
}

export interface OrderLinePayload {
  productId: number;
  quantite: number;
}

export interface OrderPayload {
  clientId: number;
  lignes: OrderLinePayload[];
}
