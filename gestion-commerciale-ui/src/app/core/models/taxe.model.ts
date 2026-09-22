export type TypeTaxe = 'Pourcentage' | 'MontantFixe';

export interface Taxe {
  id: number;
  libelle: string;
  type: TypeTaxe;
  valeur: number;
  active: boolean;
}

export interface TaxePayload {
  libelle: string;
  type: TypeTaxe;
  valeur: number;
  active: boolean;
}
