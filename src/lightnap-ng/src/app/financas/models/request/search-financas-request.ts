
import { PaginationRequest } from "@core";
import { FinancaTipo } from "./create-financas-request";
export interface SearchFinancasRequest extends PaginationRequest {
	descricao?: string;
	valor?: number;
	datainicio?: Date;
	datafinal?: Date;
	tipo?:FinancaTipo;
	categoria?: string;
}
