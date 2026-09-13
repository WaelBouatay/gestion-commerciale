export interface Product {
  id: number;
  reference: string;
  nom: string;
  description: string;
  prixUnitaireHT: number;
  quantiteEnStock: number;
  dateCreation: string;
}

export interface ProductPayload {
  reference: string;
  nom: string;
  description: string;
  prixUnitaireHT: number;
  quantiteEnStock: number;
}
