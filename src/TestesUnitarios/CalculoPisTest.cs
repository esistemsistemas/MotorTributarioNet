using MotorTributarioNet.Facade;
using MotorTributarioNet.Flags;
using TestCalculosTributarios.Entidade;
using Xunit;

namespace TestCalculosTributarios
{
    public class CalculoPisTest
    {

        [Fact]
        public void CalculoPis()
        {
            var produto = new Produto
            {
                PercentualPis = 1.65m,
                ValorProduto = 1000.00m,
                QuantidadeProduto = 1.000m
            };

            var facade = new FacadeCalculadoraTributacao(produto);

            var resultadoCalculoPis = facade.CalculaPis();

            Assert.Equal(1000.00m, resultadoCalculoPis.BaseCalculo);
            Assert.Equal(16.50m, resultadoCalculoPis.Valor);
        }

        [Fact]
        public void CalculoPisComDescontoIncondicional()
        {
            var produto = new Produto
            {
                PercentualPis = 1.65m,
                ValorProduto = 1000.00m,
                QuantidadeProduto = 1.000m,
                Desconto = 100.00m
            };

            var facade = new FacadeCalculadoraTributacao(produto, TipoDesconto.Incondicional);

            var resultadoCalculoPis = facade.CalculaPis();

            Assert.Equal(900.00m, resultadoCalculoPis.BaseCalculo);
            Assert.Equal(14.85m, resultadoCalculoPis.Valor);
        }

        [Fact]
        public void CalculoPisComIpi()
        {
            var produto = new Produto
            {
                PercentualPis = 1.65m,
                ValorProduto = 1000.00m,
                QuantidadeProduto = 1.000m,
                ValorIpi = 10
            };

            var facade = new FacadeCalculadoraTributacao(produto);

            var resultadoCalculoPis = facade.CalculaPis();

            Assert.Equal(1010.00m, resultadoCalculoPis.BaseCalculo);
            Assert.Equal(16.66m, decimal.Round(resultadoCalculoPis.Valor, 2));
        }

        [Fact]
        public void CalculoPisComIpiComDescontoIncondicional()
        {
            var produto = new Produto
            {
                PercentualPis = 1.65m,
                ValorProduto = 1000.00m,
                QuantidadeProduto = 1.000m,
                ValorIpi = 10,
                Desconto = 100.00m
            };

            var facade = new FacadeCalculadoraTributacao(produto, TipoDesconto.Incondicional);

            var resultadoCalculoPis = facade.CalculaPis();

            Assert.Equal(910.00m, resultadoCalculoPis.BaseCalculo);
            Assert.Equal(15.02m, decimal.Round(resultadoCalculoPis.Valor, 2));
        }

        [Fact]
        public void CalculoPisComIpiZero()
        {
            var produto = new Produto
            {
                PercentualPis = 1.65m,
                ValorProduto = 1000.00m,
                QuantidadeProduto = 1.000m,
                ValorIpi = 0
            };

            var facade = new FacadeCalculadoraTributacao(produto);

            var resultadoCalculoPis = facade.CalculaPis();

            Assert.Equal(1000.00m, resultadoCalculoPis.BaseCalculo);
            Assert.Equal(16.50m, resultadoCalculoPis.Valor);
        }

        [Fact]
        public void CalculoPisComIpiZeroComDescontoIncondicional()
        {
            var produto = new Produto
            {
                PercentualPis = 1.65m,
                ValorProduto = 1000.00m,
                QuantidadeProduto = 1.000m,
                ValorIpi = 0,
                Desconto = 100.00m
            };

            var facade = new FacadeCalculadoraTributacao(produto, TipoDesconto.Incondicional);

            var resultadoCalculoPis = facade.CalculaPis();

            Assert.Equal(900.00m, resultadoCalculoPis.BaseCalculo);
            Assert.Equal(14.85m, resultadoCalculoPis.Valor);
        }

        [Fact]
        public void Testa_Calculo_Base_Pis_Sem_ICMS()
        {
            var produto = CriaObjetoProdutoPisCofins();

            var tributacao = new FacadeCalculadoraTributacao(produto, TipoDesconto.Incondicional);
            var resultado = tributacao.CalculaPis();

            Assert.Equal(900.00m, resultado.BaseCalculo);
            Assert.Equal(14.85m, resultado.Valor);
        }

        [Fact]
        public void Testa_Calculo_Base_Cofins_Sem_ICMS()
        {
            var produto = CriaObjetoProdutoPisCofins();

            var tributacao = new FacadeCalculadoraTributacao(produto, TipoDesconto.Incondicional);
            var resultado = tributacao.CalculaCofins();
        }

        private static Produto CriaObjetoProdutoPisCofins()
        {
            var produto = new Produto();

            produto.Cst = MotorTributarioNet.Flags.Cst.Cst00;
            produto.CstPisCofins = CstPisCofins.Cst01;
            produto.Documento = Documento.NFCe;
            produto.IsServico = false;
            produto.PercentualCofins = 7.6m;
            produto.PercentualFcp = 1m;
            produto.PercentualIcms = 18;
            produto.PercentualPis = 1.65m;
            produto.QuantidadeProduto = 9;
            produto.ValorProduto = 23;
            produto.DeduzIcmsDaBaseDePisCofins = true;
            return produto;
        }

    }
}