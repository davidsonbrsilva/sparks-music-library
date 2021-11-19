# Sparks Music Library

![Workflow Status](https://img.shields.io/github/workflow/status/davidsonbrsilva/sparks-music-library/dotnet)
![Code Size](https://img.shields.io/github/languages/code-size/davidsonbrsilva/sparks-music-library)

**Sparks Music Library** é uma biblioteca de classes construída em .Net 5 para reconhecimento, extração e transposição de acordes musicais.

É muito comum encontrarmos soluções de transposição que reduzem o mapeamento de notas aos doze sons possíveis de se reproduzir na escala ocidental, fazendo com que ela contenha sustenidos ou bemóis, mas não ambos. Isso pode limitar o desejo do usuário de enxergar a enarmonia equivalente à gerada pela transposição. Por exemplo, o resultado desejado pode ser o campo harmônico de `Bb`, mas o que se obtém é o campo harmônico de `A#`.

Ambos `Bb` e `A#` são chamados de acordes enarmônicos (produzem o mesmo som, mas têm nomes diferentes) e podem desempenhar funções diferentes a depender do contexto. Por essa razão, o resultado simplificado dessas soluções de transposição pode surtir em um resultado não desejado. É aí que Sparks Music Library encontra espaço, procurando respeitar o papel desempenhado pelo acorde ao considerar resultados em bemol, sustenido, dobrado bemol e dobrado sustenido.

## Requisitos

- [.Net 5.0 SDK](https://www.nvidia.com/en-us/geforce-now/download/)
- [Visual Studio](https://visualstudio.microsoft.com/pt-br/downloads/)* (opcional, mas recomendado)

> \* .Net 5.0 SDK já incluso. Você não precisa instalá-lo se optar pelo Visual Studio.

## Instalação

Clone o repositório:

```
git clone https://github.com/davidsonbrsilva/sparks-music-library.git
```

Acesse a pasta raiz do projeto:

```
cd sparks-music-library
```

Construa a biblioteca:

```
dotnet build
```

Os arquivos de biblioteca serão gerados e estarão prontos para uso.

## Testes

Para os testes, execute o comando:

```
dotnet test
```

## Como usar

Em construção...

## Autor

Caso necessite, entre em contato com <davidsonbruno@outlook.com>.