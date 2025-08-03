using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNap.Core.AnexosFinanceiro.Dto.Response
{
    public class ArquivoDownloadDto
    {
        public Stream Stream { get; set; } = default!;
        public string ContentType { get; set; } = "application/octet-stream";
        public string NomeArquivo { get; set; } = default!;
    }

}
