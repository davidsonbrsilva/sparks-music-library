# Sparks Music Library <!-- omit in toc -->

![License](https://img.shields.io/github/license/davidsonbrsilva/sparks-music-library) ![Code Size](https://img.shields.io/github/languages/code-size/davidsonbrsilva/sparks-music-library) ![Status](https://img.shields.io/badge/status-stopped-red)

[[See In English](README.md)]

**Sparks Music Library** é uma biblioteca de classes construída para reconhecimento, extração e transposição de acordes musicais.

É muito comum encontrarmos soluções de transposição que reduzem o mapeamento de notas aos doze sons possíveis de se reproduzir na escala ocidental, fazendo com que ela contenha sustenidos ou bemóis, mas não ambos. Isso pode limitar o desejo do usuário de enxergar a enarmonia equivalente à gerada pela transposição. Por exemplo, o resultado desejado pode ser o campo harmônico de `Bb`, mas o que se obtém é o campo harmônico de `A#`.

Ambos `Bb` e `A#` são chamados de acordes enarmônicos (produzem o mesmo som, mas têm nomes diferentes) e podem desempenhar funções diferentes a depender do contexto. Por essa razão, o resultado simplificado dessas soluções de transposição pode surtir em um resultado não desejado. É aí que Sparks Music Library encontra espaço, procurando respeitar o papel desempenhado pelo acorde ao considerar resultados em bemol, sustenido, dobrado bemol e dobrado sustenido.

## Tabela de conteúdo <!-- omit in toc -->

- [Requisitos](#requisitos)
- [Instalação](#instalação)
  - [Instalação via Nuget](#instalação-via-nuget)
  - [Instalação manual](#instalação-manual)
- [Testes](#testes)
- [Guia rápido de uso](#guia-rápido-de-uso)
  - [Transposição](#transposição)
  - [Obtenção de correspondente cromático](#obtenção-de-correspondente-cromático)
  - [Extração](#extração)
  - [Reconhecimento](#reconhecimento)
  - [Otimização](#otimização)
- [Contato](#contato)
- [Licença](#licença)

## Requisitos

- [.Net 7.0 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/7.0)

## Instalação

### Instalação via Nuget

1. Certifique-se de que você já possua a origem de pacote https://nuget.pkg.github.com/davidsonbrsilva/index.json:
```
dotnet nuget list source
```

2. Se não possuir, adicione a seguinte origem de pacote:
```
dotnet nuget add source https://nuget.pkg.github.com/davidsonbrsilva/index.json -n github.com/davidsonbrsilva
```

3. Então navegue para o diretório do seu projeto e adicione a referência do pacote:
```
dotnet add package SparksMusic.Library
```

### Instalação manual

1. Clone o repositório:

```
git clone https://github.com/davidsonbrsilva/sparks-music-library.git
```

2. Acesse a pasta raiz do projeto:

```
cd sparks-music-library
```

3. Construa a biblioteca:

```
dotnet build -c Release
```

Os arquivos de biblioteca serão gerados e estarão prontos para uso.

## Testes

Para os testes, execute o comando:

```
dotnet test
```

## Guia rápido de uso

Para começar, importe a biblioteca `SparksMusic.Library` para o seu projeto:

```
using SparksMusic.Library;
```

A maioria dos métodos de SparksMusic Library lançam exceções nas operações e devem ser capturados por instruções `try-catch`. Para fins de simplicidade, considere que todas as chamadas a seguir estão envolvidas em instruções `try-catch`:

```csharp
try
{
    // SparksMusic Library operations here
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
```

### Transposição

Use o método `TransposeUp` se quiser transpor acordes para cima e `TransposeDown` se quiser transpor acordes para baixo. Ambos recebem como parâmetros a cadeia de caracteres do acorde que se deseja transpor e a quantidade de semitons para a transposição.

Transpor para cima

```csharp
Chord transposed = Transposer.TransposeUp("A", 2);
Console.WriteLine(transposed); // B
```

Transpor para baixo

```csharp
Chord transposed = Transposer.TransposeDown("A", 2);
Console.WriteLine(transposed); // G
```

Outras sobrecargas dos métodos suportam nota, acorde e lista de acordes como parâmetro.

### Obtenção de correspondente cromático

Use o método `GetChromaticCorrespondent` para obter o equivalente cromático da nota informada. Por exemplo, `A#` é retornada como `Bb` e vice-versa.

```csharp
Note note = new Note(NoteLetter.A, Accident.Sharp);
Note correspondent = Transposer.GetChromaticCorrespondent(note);
Console.WriteLine(correspondent); // Bb
```

Notas sem acidentes são retornadas como iguais à entrada. Por exemplo `A` é retornado como `A`.

```csharp
Note note = new Note(NoteLetter.A, Accident.None);
Note correspondent = Transposer.GetChromaticCorrespondent(note);
Console.WriteLine(correspondent); // A
```

### Extração

Use o método `ExtractChords` se quiser extrair acordes válidos de uma cadeia de caracteres.

```csharp
// This method extract the valid chords in the provided input text.
// In this case, only G#°, F#m7 and B/A are valid chords.
List<Chord> extractedChords = Transposer.ExtractChords("My chord sequence is G#°  F#m7  Fº+    F#m7/A###     B/A");

foreach (var chord in extractedChords)
{
    Console.WriteLine(chord);
}

// Output:
// G#°
// F#m7
// B/A
```

### Reconhecimento

Use o método `IsChord` se quiser verificar se a cadeia de caracteres fornecida corresponde a um acorde válido.

```csharp
Console.WriteLine(Transposer.IsChord("A")); // True
Console.WriteLine(Transposer.IsChord("H")); // False
Console.WriteLine(Transposer.IsChord("A#m7")); // True
Console.WriteLine(Transposer.IsChord("A+°")); // False
```

Use o método `GetValidChords` se quiser obter uma lista de objetos de acordes válidos a partir de uma lista de cadeias de caracteres.

```csharp
List<string> chordsList = new List<string>{ "A", "H", "A#m7", "A#º" };
List<Chord> validChords = Transposer.GetValidChords(chordsList);

foreach (Chord chord in validChords)
{
    Console.WriteLine(chord);
}

// Output:
// A
// A#m7
```

Use o método `HasDifferentChromaticPole` se quiser verificar se duas notas têm polo cromático diferentes. Por exemplo, duas notas `A#` e `Bb` retornam `false` pelo método, pois o primeiro pertence ao polo cromático sustenido e o segundo bemol.

```csharp
Note note1 = new Note(NoteLetter.A, Accident.Sharp);
Note note2 = new Note(NoteLetter.B, Accident.Flat);
Note note3 = new Note(NoteLetter.C, Accident.None);
Note note4 = new Note(NoteLetter.C, Accident.Sharp);

Console.WriteLine(Transposer.HasDifferentChromaticPole(note1, note2)); // True
Console.WriteLine(Transposer.HasDifferentChromaticPole(note1, note3)); // False
Console.WriteLine(Transposer.HasDifferentChromaticPole(note1, note4)); // False
```

### Otimização

Use o método `Optimize` se quiser aplicar otimizações ao acorde. Atualmente, o método realiza otimização caso a nota e a inversão do acorde sejam de polos cromáticos diferentes e retorna o novo acorde otimizado (por exemplo, `A#/Db` se transforma em `A#/C#`).

```csharp
Chord optimized = Transposer.Optimize("A#/Db");
Console.WriteLine(optimized); // A#/C#
```

## Contato

Caso necessite, entre em contato com <davidsonbruno@outlook.com>.

## Licença

[MIT](LICENSE) Copyright (c) 2021 Davidson Bruno
