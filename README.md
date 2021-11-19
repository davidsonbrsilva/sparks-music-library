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

## Guia rápido de uso

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

Outras sobrecargas dos métodos suportam nota, acorde e lista de acordes como parâmetro. Veja detalhes [aqui]().

### Obtenção de correspondente cromático

Use o método `GetChromaticCorrespondent` para obter o equivalente cromático da nota informada. Por exemplo, `A#` é retornada como `Bb` e vice-versa.

```csharp
Note note = new Note(NoteLetter.A, Accident.Sharp);
Note correspondent = Transposer.GetChromaticCorrespondent(note);
Console.WriteLine(correspondent); // Bb
```

Acordes sem acidentes são retornados como iguais à entrada. Por exemplo `A` é retornado como `A`.

```csharp
Note note = new Note(NoteLetter.A, Accident.None);
Note correspondent = Transposer.GetChromaticCorrespondent(note);
Console.WriteLine(correspondent); // A
```

### Extração

Use o método `ExtractChords` se quiser extrair acordes válidos de uma cadeia de caracteres.

```csharp
Chord transposed = Transposer.ExtractChords("A", 2);
Console.WriteLine(transposed) // G
```

### Reconhecimento

Use o método `IsChord` se quiser verificar se a cadeia de caracteres fornecida corresponde a um acorde válido.

```csharp
Console.WriteLine(Transposer.IsChord("A")) // true
Console.WriteLine(Transposer.IsChord("H")) // false
Console.WriteLine(Transposer.IsChord("A#m7")) // true
Console.WriteLine(Transposer.IsChord("A#°")) // false
```

Use o método `GetValidChords` se quiser obter uma lista de objetos de acordes válidos a partir de uma lista de cadeias de caracteres.

```csharp
List<string> chordsList = { "A", "H", "A#m7", "A#º" };
List<Chord> validChords = Transposer.GetValidChords(chordsList);

foreach (Chord chord in validChords)
{
    Console.WriteLine(chord);
}

// Saída:
// A
// A#m7
```

Use o método `HasDifferentChromaticPole` se quiser verificar se duas notas têm polo cromático diferentes. Por exemplo, duas notas `A#` e `Bb` retornam `false` pelo método, pois o primeiro pertence ao polo cromático sustenido e o segundo bemol.

```csharp
Note note1 = new Note(NoteLetter.A, Accident.Sharp);
Note note2 = new Note(NoteLetter.B, Accident.Flat);
Note note3 = new Note(NoteLetter.C, Accident.None);
Note note3 = new Note(NoteLetter.C, Accident.Sharp);

Console.WriteLine(Transposer.HasDifferentChromaticPole(note1, note2)); // false
Console.WriteLine(Transposer.HasDifferentChromaticPole(note1, note3)); // true
Console.WriteLine(Transposer.HasDifferentChromaticPole(note1, note4)); // true
```

### Otimização

Use o método `Optimize` se quiser aplicar otimizações ao acorde. Atualmente, o método realiza otimização caso a nota e a inversão do acorde sejam de polos cromáticos diferentes e retorna o novo acorde otimizado (por exemplo, `A#/Db` se transforma em `A#/C#`).

```csharp
Chord optimized = Transposer.Optimize("A#/Db");
Console.WriteLine(optimized); // A#/C#
```

## Autor

Caso necessite, entre em contato com <davidsonbruno@outlook.com>.