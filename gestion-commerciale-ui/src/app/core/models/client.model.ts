export interface Client {
  id: number;
  nom: string;
  prenomOuRaisonSociale: string;
  email: string;
  telephone: string;
  adresses: string[];
  dateCreation: string;
}

// Ce qu'on envoie pour créer/modifier un client (pas d'id ni de dateCreation)
export interface ClientPayload {
  nom: string;
  prenomOuRaisonSociale: string;
  email: string;
  telephone: string;
  adresses: string[];
}
