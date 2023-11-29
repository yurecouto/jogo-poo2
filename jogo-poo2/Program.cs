using System;
using System.Linq;
using System.Threading;

class Usuario
{
    public string Nome { get; set; }
    public int Energia { get; set; }
    public int Pontos { get; set; }
    public int Gols { get; set; }

    public Usuario(string nome)
    {
        Nome = nome;
        Energia = 10;
        Pontos = 0;
        Gols = 0;
    }
}

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Write("Digite o nome do primeiro jogador: ");
            string nomeUsuario1 = Console.ReadLine();
            Usuario user1 = new Usuario(nomeUsuario1);

            Console.Write("Haverá um segundo jogador humano? (s/n): ");
            string resposta = Console.ReadLine();
            Console.WriteLine();

            Usuario user2;
            if (resposta.ToLower() == "s")
            {
                Console.Write("Digite o nome do segundo jogador: ");
                string nomeUsuario2 = Console.ReadLine();
                user2 = new Usuario(nomeUsuario2);
            }
            else
            {
                user2 = new Usuario("Computador");
            }

            user1.Energia = 10;
            user2.Energia = 10;

            string[] cartas = { "Gol", "Pênalti", "Falta", "Cartão Amarelo", "Cartão Vermelho", "Energia" };

            while (user1.Energia > 0 || user2.Energia > 0)
            {
                for (int jogador = 0; jogador < 2; jogador++)
                {
                    Usuario jogadorAtual = (jogador == 0) ? user1 : user2;
                    Usuario adversario = (jogador == 0) ? user2 : user1;

                    if (jogadorAtual.Energia > 0)
                    {
                        Console.WriteLine("\nVez de " + (jogador == 0 ? user1.Nome : user2.Nome) + ":");

                        if (jogadorAtual.Nome == "Computador")
                        {
                            Console.WriteLine("O Computador está pensando...");
                            Thread.Sleep(2000);

                            string[] cartasComputador = SortearAcoes();
                            Console.WriteLine("Ações: " + cartasComputador[0] + " / " + cartasComputador[1] + " / " + cartasComputador[2]);

                            VerificarRodada(cartasComputador, jogadorAtual, adversario);
                        }
                        else
                        {
                            Console.Write("Aperte qualquer tecla para realizar uma ação...");
                            Console.ReadLine();

                            string[] cartasJogador = SortearAcoes();
                            Console.WriteLine("Ações: " + cartasJogador[0] + " / " + cartasJogador[1] + " / " + cartasJogador[2]);

                            VerificarRodada(cartasJogador, jogadorAtual, adversario);
                        }

                        jogadorAtual.Energia--;

                        Console.WriteLine("Energia restante - " + jogadorAtual.Nome + ": " + jogadorAtual.Energia + " | " + adversario.Nome + ": " + adversario.Energia);
                        Console.WriteLine("Gols             - " + jogadorAtual.Nome + ": " + jogadorAtual.Gols + " | " + adversario.Nome + ": " + adversario.Gols);
                        Console.WriteLine("Pontos           - " + jogadorAtual.Nome + ": " + jogadorAtual.Pontos + " | " + adversario.Nome + ": " + adversario.Pontos);
                    }
                    else
                    {
                        Console.WriteLine(jogadorAtual.Nome + " não pode realizar uma ação. Energia insuficiente.");
                    }

                    if (user1.Gols != user2.Gols && (user1.Energia == 0 || user2.Energia == 0))
                        break;
                }

                if (user1.Gols != user2.Gols && (user1.Energia == 0 || user2.Energia == 0))
                    break;
            }

            ExibirResultadoFinal(user1, user2);

            Console.Write("Digite '-1' para sair ou '0' para uma nova partida: ");
            string opcao = Console.ReadLine();

            if (opcao == "-1")
                break;
        }
    }

    static void VerificarRodada(string[] cartas, Usuario jogadorAtual, Usuario adversario)
    {
        if (cartas[0] == "Gol" && cartas[1] == "Gol" && cartas[2] == "Gol")
        {
            jogadorAtual.Gols++;
            Console.WriteLine("GOL! O jogador " + jogadorAtual.Nome + " marcou um gol!");
            Console.WriteLine("Placar atual: " + jogadorAtual.Nome + ": " + jogadorAtual.Gols + " | " + adversario.Nome + ": " + adversario.Gols);
        }
        else if (cartas[0] == "Energia" && cartas[1] == "Energia" && cartas[2] == "Energia")
        {
            jogadorAtual.Energia++;
            Console.WriteLine("O jogador " + jogadorAtual.Nome + " ganhou uma energia extra!");
            Console.WriteLine("Energia atual: " + jogadorAtual.Nome + ": " + jogadorAtual.Energia + " | " + adversario.Nome + ": " + adversario.Energia);
        }
        else if (cartas[0] == "Pênalti" && cartas[1] == "Pênalti" && cartas[2] == "Pênalti")
        {
            Console.WriteLine("Jogador " + jogadorAtual.Nome + " continuará jogando e deverá escolher outras três opções (Direita, Esquerda ou Centro).");

            string[] opcoesPênalti = { "Direita", "Esquerda", "Centro" };
            string escolhaJogador = ObterEscolha(opcoesPênalti);

            Console.WriteLine("Vez do adversário tentar defender...");

            string[] opcoesDefesa = { "Direita", "Esquerda", "Centro" };
            string escolhaAdversario = (adversario.Nome == "Computador") ? SortearOpcao(opcoesDefesa) : ObterEscolha(opcoesDefesa);

            if (escolhaJogador != escolhaAdversario)
            {
                jogadorAtual.Gols++;
                Console.WriteLine("GOL! O jogador " + jogadorAtual.Nome + " marcou um gol!");
                Console.WriteLine("Placar atual: " + jogadorAtual.Nome + ": " + jogadorAtual.Gols + " | " + adversario.Nome + ": " + adversario.Gols);
            }
            else
            {
                Console.WriteLine("DEFENDEU!!! NÃO foi contabilizado um gol.");
                Console.WriteLine("Placar atual: " + jogadorAtual.Nome + ": " + jogadorAtual.Gols + " | " + adversario.Nome + ": " + adversario.Gols);
            }
        }
        else if (cartas[0] == "Falta" && cartas[1] == "Falta" && cartas[2] == "Falta")
        {
            Console.WriteLine("Jogador " + jogadorAtual.Nome + " passa a vez para o adversário.");
        }
        else if (cartas[0] == "Cartão Amarelo" && cartas[1] == "Cartão Amarelo" && cartas[2] == "Cartão Amarelo")
        {
            jogadorAtual.Energia--;

            if (jogadorAtual.Energia == 0)
            {
                Console.WriteLine(jogadorAtual.Nome + " perdeu uma energia. No próximo cartão amarelo, perderá duas energias e passará a vez para o adversário.");
            }
            else
            {
                Console.WriteLine(jogadorAtual.Nome + " perdeu uma energia. Energia restante: " + jogadorAtual.Energia);
            }
        }
        else if (cartas[0] == "Cartão Vermelho" && cartas[1] == "Cartão Vermelho" && cartas[2] == "Cartão Vermelho")
        {
            jogadorAtual.Energia -= 2;

            if (jogadorAtual.Energia <= 0)
            {
                Console.WriteLine(jogadorAtual.Nome + " perdeu duas energias e passa a vez para o adversário.");
            }
            else
            {
                Console.WriteLine(jogadorAtual.Nome + " perdeu duas energias. Energia restante: " + jogadorAtual.Energia);
            }
        }
        else
        {
            int pontosRodada = CalcularPontos(cartas, jogadorAtual);
            Console.WriteLine("Pontuação da rodada: " + pontosRodada + " pontos.");
        }
    }

    static int CalcularPontos(string[] cartas, Usuario jogadorAtual)
    {
        int pontos = 0;
        foreach (var carta in cartas)
        {
            switch (carta)
            {
                case "Gol":
                    pontos += 3;
                    break;
                case "Pênalti":
                    pontos += 2;
                    break;
                case "Falta":
                case "Cartão Amarelo":
                    pontos += 1;
                    break;
                case "Cartão Vermelho":
                    break; // Não adiciona pontos
                case "Energia":
                    pontos += 2;
                    break;
            }
        }
        jogadorAtual.Pontos += pontos;
        return pontos;
    }

    static string[] SortearAcoes()
    {
        string[] cartas = { "Gol", "Pênalti", "Falta", "Cartão Amarelo", "Cartão Vermelho", "Energia" };
        return cartas.OrderBy(x => Guid.NewGuid()).ToArray();
    }

    static string SortearOpcao(string[] opcoes)
    {
        return opcoes[new Random().Next(opcoes.Length)];
    }

    static string ObterEscolha(string[] opcoes)
    {
        Console.WriteLine("Escolha uma opção:");
        for (int i = 0; i < opcoes.Length; i++)
        {
            Console.WriteLine((i + 1) + ". " + opcoes[i]);
        }

        int escolha;
        while (true)
        {
            Console.Write("Digite o número da opção: ");
            if (int.TryParse(Console.ReadLine(), out escolha) && escolha >= 1 && escolha <= opcoes.Length)
            {
                break;
            }
            Console.WriteLine("Escolha inválida. Tente novamente.");
        }

        return opcoes[escolha - 1];
    }

    static void ExibirResultadoFinal(Usuario user1, Usuario user2)
    {
        Console.WriteLine("Partida finalizada!");

        if (user1.Gols > user2.Gols)
        {
            Console.WriteLine("PARABÉNS JOGADOR 1");
            Console.WriteLine("VOCÊ VENCEU COM " + user1.Gols + " GOLS E " + user1.Pontos + " PONTOS.");
            Console.WriteLine("O SEU ADVERSÁRIO FEZ " + user2.Gols + " GOLS E " + user2.Pontos + " PONTOS");
        }
        else if (user2.Gols > user1.Gols)
        {
            Console.WriteLine("PARABÉNS JOGADOR 2");
            Console.WriteLine("VOCÊ VENCEU COM " + user2.Gols + " GOLS E " + user2.Pontos + " PONTOS.");
            Console.WriteLine("O SEU ADVERSÁRIO FEZ " + user1.Gols + " GOLS E " + user1.Pontos + " PONTOS");
        }
        else
        {
            if (user1.Pontos > user2.Pontos)
            {
                Console.WriteLine("A PARTIDA EMPATOU!");
                Console.WriteLine("Ambos jogadores fizeram a mesma quantidade de gols, mas o JOGADOR 1 venceu pelo critério de pontuação.");
            }
            else if (user2.Pontos > user1.Pontos)
            {
                Console.WriteLine("A PARTIDA EMPATOU!");
                Console.WriteLine("Ambos jogadores fizeram a mesma quantidade de gols, mas o JOGADOR 2 venceu pelo critério de pontuação.");
            }
            else
            {
                Console.WriteLine("A PARTIDA EMPATOU!");
                Console.WriteLine("Ambos jogadores fizeram a mesma quantidade de gols e a mesma pontuação.");
            }
        }
    }
}