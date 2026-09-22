export type TypeRemise = 'Pourcentage' | 'MontantFixe';

export interface Remise {
  id: number;
  libelle: string;
  type: TypeRemise;
  valeur: number;
  active: boolean;
}

export interface RemisePayload {
  libelle: string;
  type: TypeRemise;
  valeur: number;
  active: boolean;
}
