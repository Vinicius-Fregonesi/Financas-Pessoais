import { FinancaTipo } from "../request/create-financas-request";

export interface Financas {
	// TODO: Update these fields to match the server's FinancasDto.
	id: number;
	descricao: string;
	valor: number;
	data: Date;
	tipo:FinancaTipo;
	categoria: string;
}
