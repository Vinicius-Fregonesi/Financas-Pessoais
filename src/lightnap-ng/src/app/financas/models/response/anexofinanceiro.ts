export interface AnexoFinanceiroDto {
  id: number; // ← Adicione esta linha
  nomeArquivo: string;
  caminho: string;
  tipoArquivo: string;
  dataEnvio: string; // ou Date, se quiser converter
}
