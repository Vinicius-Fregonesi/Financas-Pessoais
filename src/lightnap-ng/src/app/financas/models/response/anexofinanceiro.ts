export interface AnexoFinanceiroDto {
  nomeArquivo: string;
  caminho: string;
  tipoArquivo: string;
  dataEnvio: string; // ou Date, conforme o backend retorna
}
