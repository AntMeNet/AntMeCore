using System;

namespace AntMe.Generator
{
    internal sealed class ModpackGeneratorHost : MarshalByRefObject
    {
        public ModpackGeneratorHost() { }

        public string Generate(string[] inportPathes, string outputPath)
        {
            string result = outputPath + "summary.dll";

            #region Generate Basic-Types

            //  (Compass, SpielerAttribut, Environment, Random)

            //    /// <summary>
            //    /// Liste von Kompass-Richtungsangaben.
            //    /// </summary>
            //public enum Kompass
            //{
            //    Osten = 0,
            //    Südosten = 45,
            //    Süden = 90,
            //    Südwesten = 135,
            //    Westen = 180,
            //    Nordwesten = 225,
            //    Norden = 270,
            //    Nordosten = 315
            //}

            //    /// <summary>
            //    /// Attribute to mark a Player Fabric.
            //    /// </summary>
            //[Serializable]
            //[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
            //[PlayerAttributeMapping(NameProperty = "Name", AuthorProperty = "Autor")]
            //public sealed class SpielerAttribute : AntMe.Simulation.PlayerAttribute
            //{
            //    public string Name { get; set; }
            //    public string Autor { get; set; }
            //}

            //    public sealed class Umgebung
            //{
            //    private VisibleEnvironment visibleEnvironment;

            //    /// <summary>
            //    /// Informationen zum aktuellen Standort.
            //    /// </summary>
            //    public Zelle? Zentrum { get { return Get(visibleEnvironment.Center); } }

            //    /// <summary>
            //    /// Umgebungsinformationen zum Norden.
            //    /// </summary>
            //    public Zelle? Norden { get { return Get(visibleEnvironment.North); } }

            //    /// <summary>
            //    /// Umgebungsinformationen zum Nordosten.
            //    /// </summary>
            //    public Zelle? NordOsten { get { return Get(visibleEnvironment.NorthEast); } }

            //    /// <summary>
            //    /// Umgebungsinformationen zum Osten.
            //    /// </summary>
            //    public Zelle? Osten { get { return Get(visibleEnvironment.East); } }

            //    /// <summary>
            //    /// Umgebungsinformationen zum Südosten.
            //    /// </summary>
            //    public Zelle? SüdOsten { get { return Get(visibleEnvironment.SouthEast); } }

            //    /// <summary>
            //    /// Umgebungsinformationen zum Süden.
            //    /// </summary>
            //    public Zelle? Süden { get { return Get(visibleEnvironment.South); } }

            //    /// <summary>
            //    /// Umgebungsinforamtionen zum Südwesten.
            //    /// </summary>
            //    public Zelle? SüdWesten { get { return Get(visibleEnvironment.SouthWest); } }

            //    /// <summary>
            //    /// Umgebungsinformationen zum Westen.
            //    /// </summary>
            //    public Zelle? Westen { get { return Get(visibleEnvironment.West); } }

            //    /// <summary>
            //    /// Umgebungsinformationen zum Nordwesten.
            //    /// </summary>
            //    public Zelle? NordWesten { get { return Get(visibleEnvironment.NorthWest); } }


            //    internal Umgebung(VisibleEnvironment environment)
            //    {
            //        visibleEnvironment = environment;
            //    }

            //    private Zelle? Get(VisibleCell? cell)
            //    {
            //        // Leere Zelle
            //        if (!cell.HasValue)
            //            return null;

            //        Zelle z = new Zelle();
            //        z.Geschwindigkeit = cell.Value.Speed;
            //        switch (cell.Value.Height)
            //        {
            //            case TileHeight.High: z.Höhendifferenz = Höhendifferenz.Höher; break;
            //            case TileHeight.Low: z.Höhendifferenz = Höhendifferenz.Niedriger; break;
            //            default: z.Höhendifferenz = Höhendifferenz.Gleich; break;
            //        }

            //        return z;
            //    }
            //}

            ///// <summary>
            ///// Richtungsinformation.
            ///// </summary>
            //public struct Zelle
            //{
            //    /// <summary>
            //    /// Die Geschwindigkeitsdifferenz zur aktuellen Zelle.
            //    /// </summary>
            //    public float Geschwindigkeit;

            //    /// <summary>
            //    /// Die Höhendifferenz zur aktuellen Zelle.
            //    /// </summary>
            //    public Höhendifferenz Höhendifferenz;
            //}

            //public enum Höhendifferenz
            //{
            //    Niedriger,
            //    Gleich,
            //    Höher
            //}

            //    public sealed class Zufall
            //{
            //    private Random random;

            //    internal Zufall(Random random)
            //    {
            //        if (random == null)
            //            throw new ArgumentNullException();

            //        this.random = random;
            //    }

            //    /// <summary>
            //    /// Erzeugt eine Zufallszahl zwischen 0 und der angegebenen Maximalen.
            //    /// </summary>
            //    /// <param name="maximal">Maximaler Wert</param>
            //    /// <returns>Zufallszahl</returns>
            //    public int Zahl(int maximal)
            //    {
            //        return random.Next(maximal);
            //    }

            //    /// <summary>
            //    /// Erzeugt eine Zufallszahl zwischen dem Minimum und dem Maximum.
            //    /// </summary>
            //    /// <param name="minimal">Minimaler Wert</param>
            //    /// <param name="maximal">Maximaler Wert</param>
            //    /// <returns>Zufallszahl</returns>
            //    public int Zahl(int minimal, int maximal)
            //    {
            //        return random.Next(minimal, maximal);
            //    }
            //}

            #endregion

            #region Generate Items (Info-Wrapper, including InfoProperties)

            //    /// <summary>
            //    /// Repräsentiert ein beliebiges Spielelement.
            //    /// </summary>
            //public class Etwas
            //{
            //    public Etwas(ItemInfo info)
            //    {
            //        Info = info;
            //    }

            //    internal ItemInfo Info { get; private set; }

            //    /// <summary>
            //    /// Gibt die Entfernung zu diesem Element an.
            //    /// </summary>
            //    public float Entfernung
            //    {
            //        get { return Info.Distance; }
            //    }

            //    /// <summary>
            //    /// Gibt die Richtung zu diesem Element an.
            //    /// </summary>
            //    public int Richtung
            //    {
            //        get { return Info.Direction; }
            //    }

            //    /// <summary>
            //    /// Gibt den Radius des Objektes zurück.
            //    /// </summary>
            //    public float Radius
            //    {
            //        get { return Info.Radius; }
            //    }
            //}

            //    /// <summary>
            //    /// Repräsentiert einen Zuckerberg.
            //    /// </summary>
            //public sealed class Zucker : Etwas
            //{
            //    private SugarInfo info;

            //    public Zucker(SugarInfo info)
            //        : base(info)
            //    {
            //        this.info = info;
            //    }

            //    /// <summary>
            //    /// Die Menge an verfügbarem Zucker.
            //    /// </summary>
            //    public int Menge { get { return info.Amount; } }
            //}

            #endregion

            #region Generate Faction Base-Classes

            //    public abstract class BasisKolonie : PrimordialAntColony
            //{
            //    protected AntColonyInterop Interop { get; private set; }

            //    #region Allgemein

            //    /// <summary>
            //    /// Gibt die Anzahl in 100 Runden zurück, die als "kürzlich" betrachtet 
            //    /// werden soll oder legt diese fest.
            //    /// </summary>
            //    protected int KürzlicheZeitspanne
            //    {
            //        get { return Interop.RecentCenturies; }
            //        set { Interop.RecentCenturies = value; }
            //    }

            //    /// <summary>
            //    /// Gibt die Zeit zurück, die seit dem Start der Simulation im Spiel 
            //    /// vergangen ist.
            //    /// </summary>
            //    protected TimeSpan Spielzeit { get { return Interop.GameTime; } }

            //    #endregion

            //    #region Ameisen

            //    #region Gesamtzahlen

            //    /// <summary>
            //    /// Gibt die Zahl der Ameisen zurück, die seit dem Start des Spiels erzeugten wurden.
            //    /// </summary>
            //    protected int GesamtanzahlAmeisen { get { return Interop.TotalAnts; } }

            //    /// <summary>
            //    /// Gibt die Anzahl der Ameisen (gruppiert nach Typen) zurück, die seit dem Start des Spiels erzeugt wurden.
            //    /// </summary>
            //    protected ReadOnlyDictionary<Type, int> GesamtanzahlAmeisenNachTyp
            //    {
            //        get { return Interop.TotalAntsPerType; }
            //    }

            //    /// <summary>
            //    /// Gibt die Anzahl der Ameisen (gruppiert nach Kasten) zurück, die seit dem Start des Spiels erzeugt wurden.
            //    /// </summary>
            //    protected ReadOnlyDictionary<string, int> GesamtanzahlAmeisenNachKaste
            //    {
            //        get { return Interop.TotalAntsPerCaste; }
            //    }

            //    #endregion

            //    #region Aktuelle Zahlen

            //    /// <summary>
            //    /// Gibt die Anzahl der Ameisen zurück, die aktuell im Spiel sind.
            //    /// </summary>
            //    protected int AktuelleAnzahlAmeisen { get { return Interop.CurrentAnts; } }

            //    /// <summary>
            //    /// Gibt die Anzahl der Ameisen nach Typ zurück, die aktuell im Spiel sind.
            //    /// </summary>
            //    protected ReadOnlyDictionary<Type, int> AktuelleAnzahlAmeisenNachTyp
            //    {
            //        get { return Interop.CurrentAntsPerType; }
            //    }

            //    /// <summary>
            //    /// Gibt die Anzahl der Ameisen nach Kaste zurück, die aktuell im Spiel sind.
            //    /// </summary>
            //    protected ReadOnlyDictionary<string, int> AktuelleAnzahlAmeisenNachKaste
            //    {
            //        get { return Interop.CurrentAntsPerCaste; }
            //    }

            //    #endregion

            //    #region Kürzlich

            //    /// <summary>
            //    /// Gibt die Anzahl der Ameisen zurück, die kürzlich im Spiel erstellt wurden.
            //    /// </summary>
            //    protected int KürzlicheAnzahlAmeisen { get { return Interop.RecentAnts; } }

            //    /// <summary>
            //    /// Gibt die Anzahl Ameisen nach Typ zurück, die kürzlich im Spiel erstellt wurden.
            //    /// </summary>
            //    protected ReadOnlyDictionary<Type, int> KürzlicheAnzahlAmeisenNachTyp
            //    {
            //        get { return Interop.RecentAntsPerType; }
            //    }

            //    /// <summary>
            //    /// Gibt die Anzahl Ameisen nach Kaste zurück, die kürzlich im Spiel erstellt wurden.
            //    /// </summary>
            //    protected ReadOnlyDictionary<string, int> KürzlicheAnzahlAmeisenNachKaste
            //    {
            //        get { return Interop.RecentAntsPerCaste; }
            //    }

            //    #endregion

            //    #endregion

            //    public override void Init(AntColonyInterop interop)
            //    {
            //        Interop = interop;
            //        Interop.OnCreateMember += _interop_OnCreateMember;
            //    }

            //    private Type _interop_OnCreateMember()
            //    {
            //        return ErzeugeAmeise();
            //    }

            //    public abstract Type ErzeugeAmeise();
            //}

            //    [LocalizedClass(Culture = "de")]
            //public abstract class BasisAmeise : PrimordialAnt
            //{
            //    private AntInterop _interop;
            //    private PhysicsInterop _physics;
            //    private RecognitionInterop _recognition;
            //    private InteractionInterop _interaction;

            //    public override void Init(AntInterop interop)
            //    {
            //        _interop = interop;
            //        _interop.Tick += Tick;
            //        Zufall = new Zufall(interop.Random);

            //        #region Physics

            //        _physics = _interop.GetProperty<PhysicsInterop>();
            //        if (_physics == null)
            //            throw new ArgumentException("There is no PhysicsInterop Element");

            //        // Wartet
            //        _physics.OnWaits += Wartet;

            //        // Zusammenstoß mit der Wand
            //        _physics.OnHitWall += parameter => Zusammengestoßen((Kompass)parameter);

            //        // Zusammenstöße mit anderen Elementen
            //        _physics.OnCollision += () =>
            //        {
            //            // Collided Anthills
            //            var anthill = _physics.CollidedItems.FirstOrDefault(i => i is AnthillInfo);
            //            if (anthill != null)
            //                Zusammengestoßen(new Ameisenhügel(anthill as AnthillInfo));

            //            // Collided Ants
            //            var ant = _physics.CollidedItems.FirstOrDefault(i => i is AntInfo);
            //            if (ant != null)
            //                Zusammengestoßen(new Ameise(ant as AntInfo));

            //            // Collided Sugar
            //            var sugar = _physics.CollidedItems.FirstOrDefault(i => i is SugarInfo);
            //            if (sugar != null)
            //                Zusammengestoßen(new Zucker(sugar as SugarInfo));

            //            // Collided Apple
            //            var apple = _physics.CollidedItems.FirstOrDefault(i => i is AppleInfo);
            //            if (apple != null)
            //                Zusammengestoßen(new Apfel(apple as AppleInfo));

            //            // Collided B u g
            //            var bug = _physics.CollidedItems.FirstOrDefault(i => i is BugInfo);
            //            if (bug != null)
            //                Zusammengestoßen(new Wanze(bug as BugInfo));
            //        };

            //        // Reached Target
            //        _physics.OnTargetReched += parameter =>
            //        {
            //            // Ameisenhügel erreicht
            //            if (parameter is AnthillInfo)
            //                Erreicht(new Ameisenhügel(parameter as AnthillInfo));

            //            // Ameise erreicht
            //            if (parameter is AntInfo)
            //                Erreicht(new Ameise(parameter as AntInfo));

            //            // Zucker erreicht
            //            if (parameter is SugarInfo)
            //                Erreicht(new Zucker(parameter as SugarInfo));

            //            // Apfel erreicht
            //            if (parameter is AppleInfo)
            //                Erreicht(new Apfel(parameter as AppleInfo));

            //            // Wanze erreicht
            //            if (parameter is BugInfo)
            //                Erreicht(new Wanze(parameter as BugInfo));
            //        };

            //        #endregion

            //        #region Recognition

            //        _recognition = _interop.GetProperty<RecognitionInterop>();
            //        if (_recognition == null)
            //            throw new ArgumentException("There is no RecognitionInterop Element");

            //        // Veränderung der Umgebung
            //        _recognition.OnEnvironmentChanged += parameter => VeränderteUmgebung(Umgebung);

            //        _recognition.Spots += () =>
            //        {
            //            // Spotting Anthills
            //            var anthill = _recognition.VisibleItems.FirstOrDefault(i => i is AnthillInfo);
            //            if (anthill != null)
            //                Sieht(new Ameisenhügel(anthill as AnthillInfo));

            //            // Spotting Ants
            //            var ant = _recognition.VisibleItems.FirstOrDefault(i => i is AntInfo);
            //            if (ant != null)
            //                Sieht(new Ameise(ant as AntInfo));

            //            // Spotting Sugar
            //            var sugar = _recognition.VisibleItems.FirstOrDefault(i => i is SugarInfo);
            //            if (sugar != null)
            //                Sieht(new Zucker(sugar as SugarInfo));

            //            // Spotting Apple
            //            var apple = _recognition.VisibleItems.FirstOrDefault(i => i is AppleInfo);
            //            if (apple != null)
            //                Sieht(new Apfel(apple as AppleInfo));

            //            // Spotting Bugs
            //            var bug = _recognition.VisibleItems.FirstOrDefault(i => i is BugInfo);
            //            if (bug != null)
            //                Sieht(new Wanze(bug as BugInfo));
            //        };

            //        _recognition.Smells += () =>
            //        {
            //            var marker = _recognition.SmellableItems.FirstOrDefault(i => i is MarkerInfo);
            //            if (marker != null)
            //                Riecht(new Markierung(marker as MarkerInfo));

            //        };

            //        #endregion

            //        #region Interaction

            //        _interaction = _interop.GetProperty<InteractionInterop>();
            //        if (_interaction == null)
            //            throw new ArgumentException("There is no InteractionInterop Element");

            //        // Angriffe
            //        _interaction.OnHit += parameter => WirdAngegriffen();

            //        // Tod durch Angriff
            //        _interaction.OnKill += Getötet;

            //        #endregion
            //    }

            //    #region Spieler Eigenschaften

            //    /// <summary>
            //    /// Zufallsgenerator
            //    /// </summary>
            //    public Zufall Zufall { get; private set; }

            //    /// <summary>
            //    /// Gibt das aktuelle Ziel der Ameise zurück.
            //    /// </summary>
            //    public Etwas AktuellesZiel
            //    {
            //        get
            //        {
            //            return _physics.CurrentTarget != null ? new Etwas(_physics.CurrentTarget) : null;
            //        }
            //    }

            //    /// <summary>
            //    /// Liefert alle Elemente zurück, die die Ameise aktuell berührt.
            //    /// </summary>
            //    public IEnumerable<Etwas> BerührteElemente
            //    {
            //        // TODO: bessere Loka-Hüllen (damit Prüfung auf "is Ameise" funktioniert.
            //        get { return _physics.CollidedItems.Select(i => new Etwas(i)); }
            //    }

            //    /// <summary>
            //    /// Listet alle aktuell sichtbaren Elemente auf.
            //    /// </summary>
            //    public IEnumerable<Etwas> SichtbareElemente
            //    {
            //        // TODO: bessere Loka-Hüllen (damit Prüfung auf "is Ameise" funktioniert.
            //        get { return _recognition.VisibleItems.Select(i => new Etwas(i)); }
            //    }

            //    /// <summary>
            //    /// Liefert alle riechbaren Elemente zurück.
            //    /// </summary>
            //    public IEnumerable<Etwas> RiechbareElemente
            //    {
            //        // TODO: bessere Loka-Hüllen (damit Prüfung auf "is Ameise" funktioniert.
            //        get { return _recognition.SmellableItems.Select(i => new Etwas(i)); }
            //    }

            //    /// <summary>
            //    /// Liefert eine Liste der Elemente zurück, die die Ameise aktuell angreifen.
            //    /// </summary>
            //    public IEnumerable<Etwas> AngreifendeElemente
            //    {
            //        // TODO: bessere Loka-Hüllen (damit Prüfung auf "is Ameise" funktioniert.
            //        get { return _interaction.AttackingItems.Select(i => new Etwas(i)); }
            //    }

            //    /// <summary>
            //    /// Gibt die aktuelle Beladung mit Zucker zurück.
            //    /// </summary>
            //    public int AktuelleZuckerLast
            //    {
            //        get { return _interaction.SugarLoad; }
            //    }

            //    public int MaximaleZuckerLast
            //    {
            //        get { return _interaction.MaximumSugarLoad; }
            //    }

            //    public int AktuelleApfelLast
            //    {
            //        get { return _interaction.AppleLoad; }
            //    }

            //    public int MaximaleApfelLast
            //    {
            //        get { return _interaction.MaximumAppleLoad; }
            //    }

            //    /// <summary>
            //    /// Gibt das Objekt zurück, das aktuell getragen wird.
            //    /// </summary>
            //    public Etwas AktuelleLast
            //    {
            //        get { return (_interaction.CurrentLoad != null ? new Etwas(_interaction.CurrentLoad) : null); }
            //    }

            //    /// <summary>
            //    /// Aktuelle Richtung der Ameise.
            //    /// </summary>
            //    public int Richtung
            //    {
            //        get { return _interop.Orientation.Degree; }
            //    }

            //    public Umgebung Umgebung
            //    {
            //        get { return new Umgebung(_recognition.Environment); }
            //    }

            //    #endregion

            //    #region Spieler Ereignisse

            //    /// <summary>
            //    /// Wird von der Simulation aufgerufen, wenn die Ameise aktuell nichts 
            //    /// zu tun hat.
            //    /// </summary>
            //    public virtual void Wartet()
            //    {
            //    }

            //    /// <summary>
            //    /// Wird von der Simulation in jeder Berechnungsrunde aufgerufen.
            //    /// </summary>
            //    public virtual void Tick()
            //    {
            //    }

            //    /// <summary>
            //    /// Wird von der Simulation aufgerufen, wenn die Ameise mindestens einen 
            //    /// Ameisenhügel erspäht hat.
            //    /// </summary>
            //    /// <param name="ameisenhügel">Erster Ameisenhügel</param>
            //    public virtual void Sieht(Ameisenhügel ameisenhügel)
            //    {
            //    }

            //    /// <summary>
            //    /// Wird von der Simulation aufgerufen, wenn die Ameise mindestens eine 
            //    /// Ameise in der Nähe gesehen hat.
            //    /// </summary>
            //    /// <param name="ameise">Erste Ameise</param>
            //    public virtual void Sieht(Ameise ameise)
            //    {
            //    }

            //    /// <summary>
            //    /// Wird von der Simulation aufgerufen, wenn die Ameise mindestens einen 
            //    /// Zuckerberg erspäht hat.
            //    /// </summary>
            //    /// <param name="zucker">Erster Zucker</param>
            //    public virtual void Sieht(Zucker zucker)
            //    {
            //    }

            //    /// <summary>
            //    /// Wird von der Simulation aufgerufen, wenn die Ameise mindestens einen 
            //    /// Apfel gesehen hat.
            //    /// </summary>
            //    /// <param name="apfel">Erster Apfel</param>
            //    public virtual void Sieht(Apfel apfel)
            //    {
            //    }

            //    /// <summary>
            //    /// Wird von der Simulation aufgerufen, wenn die Simulation mindestens eine 
            //    /// Wanze in der Nähe gesehen hat.
            //    /// </summary>
            //    /// <param name="wanze">Erste Wanze</param>
            //    public virtual void Sieht(Wanze wanze)
            //    {
            //    }

            //    /// <summary>
            //    /// Signalisiert den Zusammenstoß mit einem Ameisenhügel
            //    /// </summary>
            //    /// <param name="ameisenhügel">Der Ameisenhügel, mit dem die Ameise zusammen gestoßen ist</param>
            //    public virtual void Zusammengestoßen(Ameisenhügel ameisenhügel)
            //    {
            //    }

            //    /// <summary>
            //    /// Signalisiert den Zusammenstoß mit einer Ameise
            //    /// </summary>
            //    /// <param name="ameise">Der Ameise, mit dem die Ameise zusammen gestoßen ist</param>
            //    public virtual void Zusammengestoßen(Ameise ameise)
            //    {
            //    }

            //    /// <summary>
            //    /// Signalisiert den Zusammenstoß mit einem Zuckerberg
            //    /// </summary>
            //    /// <param name="zucker">Der Zuckerberg, mit dem die Ameise zusammen gestoßen ist</param>
            //    public virtual void Zusammengestoßen(Zucker zucker)
            //    {
            //    }

            //    /// <summary>
            //    /// Signalisiert den Zusammenstoß mit einem Apfel
            //    /// </summary>
            //    /// <param name="apfel">Der Apfel, mit dem die Ameise zusammen gestoßen ist</param>
            //    public virtual void Zusammengestoßen(Apfel apfel)
            //    {
            //    }

            //    /// <summary>
            //    /// Signalisiert den Zusammenstoß mit einer Wanze.
            //    /// </summary>
            //    /// <param name="wanze">Die Wanze, mit dem die Ameise zusammen gestoßen ist</param>
            //    public virtual void Zusammengestoßen(Wanze wanze)
            //    {
            //    }

            //    /// <summary>
            //    /// Wird aufgerufen, wenn die Ameise an eine Wand stößt. Die Standard-
            //    /// Implementierung sorgt dafür, dass die Ameise wie in AntMe! 1.0 von 
            //    /// der Wand "reflektiert" wird.
            //    /// </summary>
            //    /// <param name="richtung">Richtung, in der sich die Wand befindet</param>
            //    public virtual void Zusammengestoßen(Kompass richtung)
            //    {
            //        // Standard implementierung für einen Rand-Zusammenstoß.
            //        switch (richtung)
            //        {
            //            case Kompass.Norden:
            //                _physics.TurnTo(_interop.Orientation.InvertY().Degree);
            //                break;
            //            case Kompass.Süden:
            //                _physics.TurnTo(_interop.Orientation.InvertY().Degree);
            //                break;
            //            case Kompass.Westen:
            //                _physics.TurnTo(_interop.Orientation.InvertX().Degree);
            //                break;
            //            case Kompass.Osten:
            //                _physics.TurnTo(_interop.Orientation.InvertX().Degree);
            //                break;
            //        }
            //    }

            //    public virtual void Erreicht(Ameisenhügel ameisenhügel)
            //    {
            //    }

            //    public virtual void Erreicht(Ameise ameise)
            //    {
            //    }

            //    public virtual void Erreicht(Zucker zucker)
            //    {
            //    }

            //    public virtual void Erreicht(Apfel apfel)
            //    {
            //    }

            //    public virtual void Erreicht(Wanze wanze)
            //    {
            //    }

            //    public virtual void Riecht(Markierung markierung)
            //    {
            //    }

            //    public virtual void VeränderteUmgebung(Umgebung umgebung)
            //    {
            //    }

            //    public virtual void WirdAngegriffen()
            //    {
            //    }

            //    public virtual void Getötet()
            //    {

            //    }

            //    #endregion

            //    #region Spieler Methoden

            //    /// <summary>
            //    /// Gibt der Ameise das Kommando direkt zum Ameisenbau zurück zu laufen.
            //    /// </summary>
            //    public void GeheZuBau()
            //    {
            //        _physics.GoToAnthill();
            //    }

            //    /// <summary>
            //    /// Nimmt sammelbare Ressourcen auf (z.B. Zucker).
            //    /// </summary>
            //    /// <param name="etwas"></param>
            //    public void Nimm(Etwas etwas)
            //    {
            //        _interaction.Collect(etwas.Info);
            //    }

            //    /// <summary>
            //    /// Hebt tragbare Ressourcen an (z.B. Apfel).
            //    /// </summary>
            //    /// <param name="etwas"></param>
            //    public void Trage(Etwas etwas)
            //    {
            //        _interaction.Carry(etwas.Info);
            //    }

            //    /// <summary>
            //    /// Lässt alle aufgesammelten Dinge fallen.
            //    /// </summary>
            //    public void LassFallen()
            //    {
            //        _interaction.Drop();
            //    }

            //    /// <summary>
            //    /// Sprüht eine Markierung mit der angegebenen Informationen.
            //    /// </summary>
            //    /// <param name="information">Enthaltene Informationen</param>
            //    public bool SprüheMarkierung(int information)
            //    {
            //        return _recognition.MakeMark(information, 0f);
            //    }

            //    /// <summary>
            //    /// Sprüht eine Markierung mit der angegebenen Informationen.
            //    /// </summary>
            //    /// <param name="information">Enthaltene Informationen</param>
            //    /// <param name="radius">Angestrebte Endgröße der Markerung</param>
            //    public bool SprüheMarkierung(int information, float radius)
            //    {
            //        return _recognition.MakeMark(information, radius);
            //    }

            //    /// <summary>
            //    /// Lässt die Ameise auf unbestimmte Zeit geradeaus gehen.
            //    /// </summary>
            //    public void GeheGeradeaus()
            //    {
            //        _physics.Goahead(int.MaxValue);
            //    }

            //    /// <summary>
            //    /// Lässt die Ameise die angegebene Entfernung zurücklegen.
            //    /// </summary>
            //    /// <param name="entfernung">Entfernung</param>
            //    public void GeheGeradeaus(float entfernung)
            //    {
            //        _physics.Goahead(entfernung);
            //    }

            //    /// <summary>
            //    /// Dreht die Ameise um den angegebenen Winkel. Ein negativer Winkel 
            //    /// lässt die Ameise nach links drehen, ein positiver Wert nach rechts.
            //    /// </summary>
            //    /// <param name="winkel">Winkel, um den sich die Ameise drehen soll</param>
            //    public void Drehe(int winkel)
            //    {
            //        _physics.Turn(winkel);
            //    }

            //    /// <summary>
            //    /// Dreht die Ameise in die angegebene Himmelsrichtung.
            //    /// </summary>
            //    /// <param name="richtung">Himmelsrichtung</param>
            //    public void DreheZu(int richtung)
            //    {
            //        _physics.TurnTo(richtung);
            //    }

            //    /// <summary>
            //    /// Stoppt alle aktuellen Aktivitäten der Ameise.
            //    /// </summary>
            //    public void Stop()
            //    {
            //        _physics.Stop();
            //    }

            //    /// <summary>
            //    /// Lässt die Ameise zum angegebenen Ziel marschieren.
            //    /// </summary>
            //    /// <param name="ziel">Ziel</param>
            //    public void GeheZu(Etwas ziel)
            //    {
            //        _physics.GoTo(ziel.Info);
            //    }

            //    #endregion
            //}

            #endregion

            return result;
        }
    }
}
