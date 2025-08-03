import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT } from "@core";
import { Observable } from "rxjs";
import { AnexoFinanceiroDto } from "../models/response/anexofinanceiro";

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


    downloadArquivo(anexo: AnexoFinanceiroDto): Observable<Blob> {
        return this.#http.get(`${this.#apiUrlRoot}DownloadAnexo`, {
            params: { caminho: anexo.caminho },
            responseType: 'blob'
        });
    }

    excluirAnexo(anexoId: number): Observable<void> {
        return this.#http.delete<void>(`${this.#apiUrlRoot}ExcluirAnexo/${anexoId}`);
    }

}
