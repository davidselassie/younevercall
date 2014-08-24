using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldState
{
    public HashSet<BridgeComponent> bridges;
    public HashSet<CharacterComponent> characters;
    public HashSet<IslandComponent> islands;

    public WorldState (HashSet<BridgeComponent> bridges, HashSet<CharacterComponent> characters, HashSet<IslandComponent> islands)
    {
        this.bridges = bridges;
        this.islands = islands;
        this.characters = characters;
    }

    public List<BridgeComponent> BridgesNewestToOldest ()
    {
        return this.bridges.OrderByDescending (b => b.created).ToList ();
    }

    public List<IslandComponent> ShortestBridgePath (IslandComponent from, IslandComponent to)
    {
        return this.ShortestBridgePath (from, to, new List<IslandComponent> (0));
    }
    
    public class NoPathException : Exception
    {
        public IslandComponent from;
        public IslandComponent to;
        
        public NoPathException (IslandComponent from, IslandComponent to)
        {
            this.from = from;
            this.to = to;
        }
    }
    
    private List<IslandComponent> ShortestBridgePath (IslandComponent from, IslandComponent to, List<IslandComponent> pathToFrom)
    {
        List<IslandComponent> foundPath = new List<IslandComponent> (pathToFrom);
        if (to == from) {
            // Muffin. foundPath is already correct.
        } else if (this.AllIslandsAccessibleFromIsland (from).Contains (to)) {
            foundPath.Add (to);
        } else {
            List<List<IslandComponent>> paths = new List<List<IslandComponent>> ();
            foreach (IslandComponent possibleNext in this.AllIslandsAccessibleFromIsland(from)) {
                if (!pathToFrom.Contains (possibleNext)) {
                    try {
                        List<IslandComponent> pathToPossibleNext = new List<IslandComponent> (pathToFrom);
                        pathToPossibleNext.Add (possibleNext);
                        paths.Add (this.ShortestBridgePath (possibleNext, to, pathToPossibleNext));
                    } catch (NoPathException) {
                        // That was a dead end.
                    }
                }
            }
            if (paths.Count > 0) {
                int minLength = this.islands.Count + 1;
                foreach (List<IslandComponent> path in paths) {
                    if (path.Count < minLength) {
                        foundPath = path;
                        minLength = path.Count;
                    }
                }
            } else {
                throw new NoPathException (from, to);
            }
        }
        return foundPath;
    }
    
    private HashSet<BridgeComponent> AllBridgesTouchingIsland (IslandComponent island)
    {
        HashSet<BridgeComponent> touching = new HashSet<BridgeComponent> ();
        foreach (BridgeComponent bridge in this.bridges) {
            if (bridge.Touches (island)) {
                touching.Add (bridge);
            }
        }
        return touching;
    }
    
    public HashSet<IslandComponent> AllIslandsAccessibleFromIsland (IslandComponent from)
    {
        HashSet<BridgeComponent> bridgesTouching = this.AllBridgesTouchingIsland (from);
        HashSet<IslandComponent> accessible = new HashSet<IslandComponent> ();
        foreach (BridgeComponent bridge in bridgesTouching) {
            accessible.Add (bridge.LeadsTo (from));
        }
        return accessible;
    }
}
