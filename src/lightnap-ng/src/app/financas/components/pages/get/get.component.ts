import { CommonModule } from "@angular/common";
import { Component, effect, inject, input, signal } from "@angular/core";
import { RouterLink } from "@angular/router";
import { ApiResponseComponent, ToastService } from "@core";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DialogModule } from "primeng/dialog";
import { ToastModule } from "primeng/toast";
import { Observable } from "rxjs";
import { FinancaTipo } from "src/app/financas/models/request/create-financas-request";
import { Financas } from "src/app/financas/models/response/financas";
import { FinancasService } from "src/app/financas/services/financas.service";
import { AnexoFinanceiroService } from "src/app/financas/services/anexofinanceiroservice";
import { AnexoFinanceiroDto } from "src/app/financas/models/response/anexofinanceiro";

@Component({
  standalone: true,
  templateUrl: "./get.component.html",
  imports: [
    CommonModule,
    CardModule,
    RouterLink,
    ApiResponseComponent,
    ButtonModule,
    DialogModule,
    ToastModule,
  ],
})
export class GetComponent {
  // Serviços
  #financasService = inject(FinancasService);
  #anexoService = inject(AnexoFinanceiroService);
  #toastService = inject(ToastService);

  anexos = signal<AnexoFinanceiroDto[]>([]);
  readonly id = input.required<number>();
  financas$ = new Observable<Financas>();

  // Modal & Upload
  showModal = signal(false);
  selectedFile = signal<File | null>(null);

  tipos = FinancaTipo;

  constructor() {
    effect(() => {
      const financaId = this.id();
      this.financas$ = this.#financasService.getFinancas(financaId);

      this.#anexoService.getAnexos(financaId).subscribe({
        next: (data) => this.anexos.set(data),
        error: () => this.anexos.set([]),
      });
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      if (file.type === "application/pdf" || file.type.startsWith("image/")) {
        this.selectedFile.set(file);
      } else {
        this.#toastService.warn("Apenas arquivos PDF ou Imagem são permitidos.");
        input.value = "";
        this.selectedFile.set(null);
      }
    }
  }

  uploadFile() {
    const file = this.selectedFile();
    if (!file) return;

    this.#anexoService.uploadArquivo(this.id(), file).subscribe({
      next: () => {
        this.#toastService.success("Arquivo enviado com sucesso!");
        this.showModal.set(false);
        this.selectedFile.set(null);

        this.#anexoService.getAnexos(this.id()).subscribe({
          next: (data) => this.anexos.set(data),
          error: () => this.anexos.set([]),
        });
      },
      error: (err) => {
        const mensagens = err?.errorMessages ?? ["Erro ao enviar o arquivo."];
        this.#toastService.errorMessages(mensagens);
      },
    });
  }

  baixarAnexo(anexo: AnexoFinanceiroDto) {
    this.#anexoService.downloadArquivo(anexo).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.download = anexo.nomeArquivo;
        link.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        const mensagens = err?.errorMessages ?? ["Erro ao baixar o arquivo."];
        this.#toastService.errorMessages(mensagens);
      },
    });
  }

  excluirAnexo(anexo: AnexoFinanceiroDto) {
    if (!confirm(`Deseja excluir o arquivo "${anexo.nomeArquivo}"?`)) return;

    this.#anexoService.excluirAnexo(anexo.id).subscribe({
      next: () => {
        this.#toastService.success("Arquivo excluído com sucesso!");
        this.#anexoService.getAnexos(this.id()).subscribe({
          next: (data) => this.anexos.set(data),
          error: () => this.anexos.set([]),
        });
      },
      error: (err) => {
        const mensagens = err?.errorMessages ?? ["Erro ao excluir o arquivo."];
        this.#toastService.errorMessages(mensagens);
      },
    });
  }
}
