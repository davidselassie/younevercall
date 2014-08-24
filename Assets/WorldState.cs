using System;
using System.Collections;
using System.Collections.Generic;

public class WorldState
{
    public IList<BridgeComponent> bridges;
    public IList<CharacterComponent> characters;
    public IList<IslandComponent> islands;

    public WorldState (IList<BridgeComponent> bridges, IList<CharacterComponent> characters, IList<IslandComponent> islands)
    {
        this.bridges = bridges;
        this.islands = islands;
        this.characters = characters;
    }

    public IslandComponent IslandWithLabel (string label)
    {
        foreach (IslandComponent island in this.islands) {
            if (island.label == label) {
                return island;
            }
        }
        return null;
    }

    public CharacterComponent CharacterWithLabel (string label)
    {
        foreach (CharacterComponent character in this.characters) {
            if (character.label == label) {
                return character;
            }
        }
        return null;
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
    
    private IList<BridgeComponent> AllBridgesTouchingIsland (IslandComponent island)
    {
        IList<BridgeComponent> touching = new List<BridgeComponent> (this.bridges.Count);
        foreach (BridgeComponent bridge in this.bridges) {
            if (bridge.Touches (island)) {
                touching.Add (bridge);
            }
        }
        return touching;
    }
    
    private IList<IslandComponent> AllIslandsAccessibleFromIsland (IslandComponent from)
    {
        IList<BridgeComponent> bridgesTouching = this.AllBridgesTouchingIsland (from);
        IList<IslandComponent> accessible = new List<IslandComponent> (bridgesTouching.Count);
        foreach (BridgeComponent bridge in bridgesTouching) {
            accessible.Add (bridge.LeadsTo (from));
        }
        return accessible;
    }
}
