using System.Diagnostics;

namespace Day07;

public enum Strength
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}

[DebuggerDisplay("{SourceCards} ")]
public class Hand
{

    public string SourceCards;
    public string Cards;
    public long Bid;
    public Strength Strength;

    public Hand(string input, bool useJokers)
    {
        var parts = input.Split(" ");
        this.Bid = long.Parse(parts[1]);
        this.SourceCards = parts[0];
        this.Cards = this.SourceCards
            .Replace("T", "C")
            .Replace("J", "D")
            .Replace("Q", "E")
            .Replace("K", "F")
            .Replace("A", "G");

        if (!useJokers || !this.Cards.Contains("D")) // Does not contain Joker
        {
            this.Strength = GetStrength(this.Cards);
        }
        else
        {
            var maxStrength = Strength.HighCard;
            foreach (var joker in "23456789CDEFG")
            {
                var cards = this.Cards.Replace("D", joker.ToString());
                var strength = GetStrength(cards);
                if (strength >= maxStrength)
                {
                    maxStrength = strength;
                }
            }

            this.Strength = maxStrength;
            this.Cards = this.Cards.Replace("D", "0");
        }
    }

    private static Strength GetStrength(string cards)
    {
        Strength result;

        // Count card occurrences
        var dict = cards
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        // Calculate strength
        if (dict.Any(x => x.Value == 5))
        {
            result = Strength.FiveOfAKind;
        }
        else if (dict.Any(x => x.Value == 4))
        {
            result = Strength.FourOfAKind;
        }
        else if (dict.Any(x => x.Value == 3) && dict.Count == 2)
        {
            result = Strength.FullHouse;
        }
        else if (dict.Any(x => x.Value == 3) && dict.Count == 3)
        {
            result = Strength.ThreeOfAKind;
        }
        else if (dict.Count == 3 && dict.Count(x => x.Value == 2) == 2)
        {
            result = Strength.TwoPair;
        }
        else if (dict.Count(x => x.Value == 2) == 1 && dict.Count(x => x.Value == 1) == 3)
        {
            result = Strength.OnePair;
        }
        else if (dict.Count == 5)
        {
            result = Strength.HighCard;
        }
        else
        {
            throw new NotImplementedException();
        }

        return result;
    }
}