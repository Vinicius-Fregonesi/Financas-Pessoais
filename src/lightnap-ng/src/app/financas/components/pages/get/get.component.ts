import { CommonModule } from "@angular/common";
import { Component, effect, inject, input, signal } from "@angular/core";
import { RouterLink } from "@angular/router";
import { ApiResponseComponent } from "@core";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DialogModule } from "primeng/dialog";
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
  ],
})
export class GetComponent {
  // Serviços
  #financasService = inject(FinancasService);
  #anexoService = inject(AnexoFinanceiroService);

  anexos = signal<AnexoFinanceiroDto[]>([]);
  readonly id = input.required<number>();
  financas$ = new Observable<Financas>();

  // Modal & Upload
  showModal = signal(false);
  selectedFile = signal<File | null>(null);

  tipos = FinancaTipo;
  errors: string[] = [];

  constructor() {
    effect(() => {
      const financaId = this.id();
      this.financas$ = this.#financasService.getFinancas(financaId);

      // Carrega Anexos
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
        alert("Apenas arquivos PDF ou Imagem são permitidos.");
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
        alert("Arquivo enviado com sucesso!");
        this.showModal.set(false);
        this.selectedFile.set(null);

        // Recarrega Anexos
        this.#anexoService.getAnexos(this.id()).subscribe({
          next: (data) => this.anexos.set(data),
          error: () => this.anexos.set([]),
        });
      },
      error: () => {
        alert("Erro ao enviar o arquivo.");
      },
    });
  }

  baixarAnexo(anexo: AnexoFinanceiroDto) {
    const link = document.createElement('a');
    link.href = anexo.caminho; // Exemplo: /Uploads/2009/Anexos/arquivo.pdf
    link.download = anexo.nomeArquivo;
    link.target = '_blank';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}
