using Tools;

var lines = File.ReadAllLines("data.txt").ToList();

var extendedLines = new List<string> { new('.', lines.Count + 2) };
lines.ForEach(line => extendedLines.Add("." + line + "."));
extendedLines.Add(new string('.', lines.Count + 2));

var map = new Matrix<char>(extendedLines.Select(line => line.ToCharArray().ToList()).ToList());
var startCoordinate = map.GetFirst('S');

var initialSearchDirections = new List<Vector> { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };

var answerA = PartA();
var answerB = PartB();
return;

long PartA()
{
    long maxLoopLength = 0;
    foreach (var searchDirection in initialSearchDirections)
    {
        var loopLength = GetLoopLength(searchDirection);
        maxLoopLength = Math.Max(maxLoopLength, loopLength);
    }

    var maxDistance = (long)Math.Ceiling(maxLoopLength / 2.0);
    return maxDistance;

    long GetLoopLength(Vector initialSearchDirection)
    {
        var counter = 0;
        var previousAnimalLocation = startCoordinate;
        var currentAnimalLocation = startCoordinate;
        while (counter == 0 || !Equals(currentAnimalLocation, startCoordinate))
        {
            if (GetNextCoordinateIfValid(previousAnimalLocation, currentAnimalLocation, initialSearchDirection) is not Vector nextAnimalLocation) return 0;

            counter++;
            previousAnimalLocation = currentAnimalLocation;
            currentAnimalLocation = nextAnimalLocation;
        }

        return counter;
    }
}

long PartB()
{
    List<long> tileCounts = new List<long>();
    foreach (var searchDirection in initialSearchDirections)
    {
        var tilCount = GetEnclosedTileCount(searchDirection);
        tileCounts.Add(tilCount);
    }

    return 0;

    long GetEnclosedTileCount(Vector initialSearchDirection)
    {
        var loop = new List<Vector>() { startCoordinate };
        var enclosedTiles = new List<Vector>();

        var counter = 0;
        var previousAnimalLocation = startCoordinate;
        var currentAnimalLocation = startCoordinate;
        while (counter == 0 || !Equals(currentAnimalLocation, startCoordinate))
        {
            var difference = currentAnimalLocation - previousAnimalLocation;
            var currentPipe = map.GetValueAt(currentAnimalLocation);

            if (currentPipe == '|')
            {
                if (difference.X == 1) enclosedTiles.Add(currentAnimalLocation + new Vector(0, -1));
                else if (difference.X == -1) enclosedTiles.Add(currentAnimalLocation + new Vector(0, 1));
            }
            else if (currentPipe == '-')
            {
                if (difference.Y == 1) enclosedTiles.Add(currentAnimalLocation + new Vector(1, 0));
                else if (difference.Y == -1) enclosedTiles.Add(currentAnimalLocation + new Vector(-1, 0));
            }
            else if (currentPipe == 'L')
            {
                if (difference.X == 1)
                {
                    enclosedTiles.Add(currentAnimalLocation + new Vector(1, 0));
                    enclosedTiles.Add(currentAnimalLocation + new Vector(0, -1));
                }
                else if (difference.Y == -1)
                {
                    enclosedTiles.Add(currentAnimalLocation + new Vector(-1, 0));
                    enclosedTiles.Add(currentAnimalLocation + new Vector(0, 1));
                }
            }
            else if (currentPipe == 'J')
            {
                if (difference.X == 1)
                {
                    enclosedTiles.Add(currentAnimalLocation + new Vector(-1, 0));
                    enclosedTiles.Add(currentAnimalLocation + new Vector(0, -1));
                }
                else if (difference.Y == 1)
                {
                    enclosedTiles.Add(currentAnimalLocation + new Vector(1, 0));
                    enclosedTiles.Add(currentAnimalLocation + new Vector(0, 1));
                }
            }
            else if (currentPipe == '7')
            {
                if (difference.X == -1)
                {
                    enclosedTiles.Add(currentAnimalLocation + new Vector(-1, 0)); 
                    enclosedTiles.Add(currentAnimalLocation + new Vector(0, 1));
                }
                else if (difference.Y == 1)
                {
                    enclosedTiles.Add(currentAnimalLocation + new Vector(1, 0));
                    enclosedTiles.Add(currentAnimalLocation + new Vector(0, -1));
                }
            }
            else if (currentPipe == 'F')
            {
                if (difference.X == -1)
                {
                    enclosedTiles.Add(currentAnimalLocation + new Vector(1, 0)); 
                    enclosedTiles.Add(currentAnimalLocation + new Vector(0, 1));
                }
                else if (difference.Y == -1)
                {
                    enclosedTiles.Add(currentAnimalLocation + new Vector(-1, 0));
                    enclosedTiles.Add(currentAnimalLocation + new Vector(0, -1));
                }
            }

            if (GetNextCoordinateIfValid(previousAnimalLocation, currentAnimalLocation, initialSearchDirection) is not Vector nextAnimalLocation) return 0;

            loop.Add(nextAnimalLocation);

            counter++;
            previousAnimalLocation = currentAnimalLocation;
            currentAnimalLocation = nextAnimalLocation;
        }

        loop = loop.Distinct().ToList();
        enclosedTiles = enclosedTiles.Distinct().ToList();

        enclosedTiles = enclosedTiles.Where(x => !loop.Contains(x)).ToList();

        var enclosedCount = 0;
        while (enclosedTiles.Count != enclosedCount)
        {
            if (enclosedCount > 5000) break;

            enclosedCount = enclosedTiles.Count;

            var neighborTiles = new List<Vector>();

            foreach (var tile in enclosedTiles)
            {
                foreach (var direction in initialSearchDirections)
                {
                    var neighbor = tile + direction;
                    if (!enclosedTiles.Contains(neighbor) && !neighborTiles.Contains(neighbor) && !loop.Contains(neighbor)
                        && neighbor.X >= 0 && neighbor.Y >= 0 && neighbor.X <= map.Rows.Count - 1 && neighbor.Y <= map.Rows[0].Count - 1)
                    {
                        neighborTiles.Add(neighbor);
                    }
                }
            }

            enclosedTiles.AddRange(neighborTiles);
            enclosedTiles = enclosedTiles.Where(x => !loop.Contains(x)).ToList();

            enclosedTiles = enclosedTiles.Distinct().ToList();
        }

        return enclosedTiles.Count;
    }
}



Vector? GetNextCoordinateIfValid(Vector previousCoordinate, Vector thisCoordinate, Vector initialSearchDirection)
{
    if (GetNextCoordinate(previousCoordinate, thisCoordinate, initialSearchDirection) is not Vector candidateCoordinate) return null;
    var isValid = CanAccessCoordinate(thisCoordinate, candidateCoordinate);
    return isValid ? candidateCoordinate : null;
}

Vector? GetNextCoordinate(Vector previousCoordinate, Vector thisCoordinate, Vector initialSearchDirection)
{
    var thisPipe = map.GetValueAt(thisCoordinate);

    switch (thisPipe)
    {
        case '.':
            throw new Exception("'.' is not a pipe.");
        case 'S':
            return thisCoordinate + initialSearchDirection;
    }

    var difference = thisCoordinate - previousCoordinate;

    if (difference.X == 1)
    {
        switch (thisPipe)
        {
            case '|':
                return thisCoordinate + new Vector(1, 0);
            case '-':
                return null;
            case 'L':
                return thisCoordinate + new Vector(0, 1);
            case 'J':
                return thisCoordinate + new Vector(0, -1);
            case '7':
                return null;
            case 'F':
                return null;
        }
    }
    else if (difference.X == -1)
    {
        switch (thisPipe)
        {
            case '|':
                return thisCoordinate + new Vector(-1, 0);
            case '-':
                return null;
            case 'L':
                return null;
            case 'J':
                return null;
            case '7':
                return thisCoordinate + new Vector(0, -1);
            case 'F':
                return thisCoordinate + new Vector(0, 1);
        }
    }
    else if (difference.Y == 1)
    {
        switch (thisPipe)
        {
            case '|':
                return null;
            case '-':
                return thisCoordinate + new Vector(0, 1);
            case 'L':
                return null;
            case 'J':
                return thisCoordinate + new Vector(-1, 0);
            case '7':
                return thisCoordinate + new Vector(1, 0);
            case 'F':
                return null;
        }
    }
    else if (difference.Y == -1)
    {
        switch (thisPipe)
        {
            case '|':
                return null;
            case '-':
                return thisCoordinate + new Vector(0, -1);
            case 'L':
                return thisCoordinate + new Vector(-1, 0);
            case 'J':
                return null;
            case '7':
                return null;
            case 'F':
                return thisCoordinate + new Vector(1, 0);
        }
    }

    throw new Exception($"Unknown pipe type {thisPipe}");
}

bool CanAccessCoordinate(Vector previousCoordinate, Vector candidateCoordinate)
{
    var candidatePipe = map.GetValueAt(candidateCoordinate);

    switch (candidatePipe)
    {
        case '.':
            return false;
        case 'S':
            return true;
    }

    var difference = candidateCoordinate - previousCoordinate;
    if (difference.ManhattanNorm() != 1) return false;

    if (difference.X == 1)
    {
        switch (candidatePipe)
        {
            case '|':
                return true;
            case '-':
                return false;
            case 'L':
                return true;
            case 'J':
                return true;
            case '7':
                return false;
            case 'F':
                return false;
        }
    }
    else if (difference.X == -1)
    {
        switch (candidatePipe)
        {
            case '|':
                return true;
            case '-':
                return false;
            case 'L':
                return false;
            case 'J':
                return false;
            case '7':
                return true;
            case 'F':
                return true;
        }
    }
    else if (difference.Y == 1)
    {
        switch (candidatePipe)
        {
            case '|':
                return false;
            case '-':
                return true;
            case 'L':
                return false;
            case 'J':
                return true;
            case '7':
                return true;
            case 'F':
                return false;
        }
    }
    else if (difference.Y == -1)
    {
        switch (candidatePipe)
        {
            case '|':
                return false;
            case '-':
                return true;
            case 'L':
                return true;
            case 'J':
                return false;
            case '7':
                return false;
            case 'F':
                return true;
        }
    }

    throw new Exception($"Unknown pipe type {candidatePipe}");
}