import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT } from "@core";
import { Observable } from "rxjs";

// DTO esperado para os anexos
export interface AnexoFinanceiroDto {
    nomeArquivo: string;
    caminho: string;
    tipoArquivo: string;
    dataEnvio: string; // ou Date, dependendo do backend
}

@Injectable({
    providedIn: "root",
})
export class AnexoFinanceiroService {
    #http = inject(HttpClient);
    #apiUrlRoot = `${inject(API_URL_ROOT)}AnexoFinanceiro/`;

    uploadArquivo(financaId: number, arquivo: File): Observable<any> {
        const formData = new FormData();
        formData.append("arquivo", arquivo);
        formData.append("financaId", financaId.toString());

        return this.#http.post(`${this.#apiUrlRoot}UploadArquivo`, formData);
    }

    getAnexos(financaId: number): Observable<AnexoFinanceiroDto[]> {
        return this.#http.get<AnexoFinanceiroDto[]>(`${this.#apiUrlRoot}ListarAnexos`, {
            params: { financasId: financaId.toString() }
        });
    }

}
