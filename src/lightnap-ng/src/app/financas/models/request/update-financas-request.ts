import { FinancaTipo } from "./create-financas-request";

export interface UpdateFinancasRequest {
	// TODO: Update these fields to match the server's UpdateFinancasDto.
	descricao: string;
	valor: number;
	data: Date;
	tipo:FinancaTipo;
	categoria: string;
}