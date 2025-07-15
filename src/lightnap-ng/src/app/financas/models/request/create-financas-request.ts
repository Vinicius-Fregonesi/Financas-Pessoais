
export interface CreateFinancasRequest {
	// TODO: Update these fields to match the server's CreateFinancasDto.
	descricao: string;
	valor: number;
	data: Date;
	tipo:FinancaTipo;
	categoria: string;
}
export enum FinancaTipo {
  Receita = 'Receita',
  Despesa = 'Despesa'
}

