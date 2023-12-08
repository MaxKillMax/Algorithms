using System.Text;

internal class Program
{
    private static void Main(string[] _)
    {

    }

    #region Quick Sorting

    private static void QuickSort(int[] array)
    {
        if (array.Length < 2)
            return;

        Random random = new();
        Sort(0, array.Length);

        void Sort(int start, int size)
        {
            int splitter = random.Next(start, start + size);
            Split(array[splitter], start, size, out int equalStart, out int rightStart);
            int countOfLeftInts = equalStart - start;
            int countOfRightInts = size - (rightStart - start);

            if (countOfLeftInts > 1)
                Sort(start, countOfLeftInts);

            if (countOfRightInts > 1)
                Sort(rightStart, countOfRightInts);
        }

        void Split(int splitter, int start, int size, out int equalStart, out int rightStart)
        {
            equalStart = start;
            rightStart = start;

            for (int i = start; i < start + size; i++)
            {
                if (array[i] == splitter)
                {
                    (array[i], array[rightStart]) = (array[rightStart], array[i]);

                    rightStart++;
                }
                else if (array[i] < splitter)
                {
                    int newElement = array[i];

                    array[i] = array[rightStart];
                    array[rightStart] = array[equalStart];
                    array[equalStart] = newElement;

                    equalStart++;
                    rightStart++;
                }
            }
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------

    #region Merge Sorting

    private static int[] MergeSort(int[] array)
    {
        SortRecursively(ref array, 0, array.Length);
        return array;

        static void SortRecursively(ref int[] array, int start, int size)
        {
            if (size < 2)
                return;

            int average = size / 2;

            int secondStart = start + average;
            int secondSize = size - average;

            SortRecursively(ref array, start, average);
            SortRecursively(ref array, secondStart, secondSize);

            int[] mergeArray = Merge(array, start, average, secondStart, secondSize);

            for (int i = 0; i < size; i++)
                array[start + i] = mergeArray[i];
        }
    }

    private static int[] Merge(int[] mergeArray, int firstStart, int firstSize, int secondStart, int secondSize)
    {
        int[] result = new int[firstSize + secondSize];

        firstSize += firstStart;
        secondSize += secondStart;

        for (int i = 0; i < result.Length; i++)
        {
            if (secondStart >= secondSize || (firstStart < firstSize && mergeArray[firstStart] <= mergeArray[secondStart]))
            {
                result[i] = mergeArray[firstStart];
                firstStart++;
            }
            else
            {
                result[i] = mergeArray[secondStart];
                secondStart++;
            }
        }

        return result;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------

    #region Bit Sorting

    private static void BitSort()
    {
        int count = Convert.ToInt32(Console.ReadLine());
        string[] numbers = new string[count];

        for (int i = 0; i < count; i++)
            numbers[i] = Console.ReadLine();

        Console.WriteLine("Initial array:");
        Console.WriteLine(string.Join(", ", numbers));
        Console.WriteLine("**********");

        numbers = Sort(numbers);

        Console.WriteLine("Sorted array:");
        Console.WriteLine(string.Join(", ", numbers));

        static string[] Sort(string[] numbers)
        {
            int length = numbers[0].Length;

            if (length == 0)
                return numbers;

            List<string>[] strings = new List<string>[10];
            List<string>[] copyStrings = new List<string>[10];

            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = new();
                copyStrings[i] = new();
            }

            length--;

            for (int i = 0; i < numbers.Length; i++)
            {
                int value = numbers[i][length] - '0';
                strings[value].Add(numbers[i]);
            }

            PrintPhase();
            length--;

            Sort();
            return GetSortedNumbers();

            void Sort()
            {
                if (length < 0)
                    return;

                for (int i = 0; i < copyStrings.Length; i++)
                    copyStrings[i].Clear();

                for (int i = 0; i < strings.Length; i++)
                {
                    for (int x = 0; x < strings[i].Count; x++)
                    {
                        int value = strings[i][x][length] - '0';
                        copyStrings[value].Add(strings[i][x]);
                    }
                }

                for (int i = 0; i < strings.Length; i++)
                {
                    strings[i].Clear();
                    strings[i].AddRange(copyStrings[i]);
                }

                PrintPhase();

                length--;

                Sort();
            }

            string[] GetSortedNumbers()
            {
                int counter = 0;

                for (int i = 0; i < strings.Length; i++)
                {
                    for (int x = 0; x < strings[i].Count; x++)
                    {
                        numbers[counter] = strings[i][x];
                        counter++;
                    }
                }

                return numbers;
            }

            void PrintPhase()
            {
                int phase = numbers[0].Length - length;

                Console.WriteLine($"Phase {phase}");

                for (int i = 0; i < strings.Length; i++)
                {
                    StringBuilder builder = new($"Bucket {i}:");

                    if (strings[i].Count == 0)
                        builder.Append(" empty");

                    for (int x = 0; x < strings[i].Count; x++)
                    {
                        if (x > 0)
                            builder.Append(',');

                        builder.Append($" {strings[i][x]}");
                    }

                    Console.WriteLine(builder);
                }

                Console.WriteLine("**********");
            }
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------

    #region Text Comparing

    private static void CompareText()
    {
        string consoleText = Console.ReadLine();
        int count = Convert.ToInt32(Console.ReadLine());

        StringBuilder stringBuilder = new();

        string text = ' ' + consoleText;

        int unknown = 257;
        long divider = 1000000007;

        long[] unknowns = new long[text.Length];
        long[] hashes = new long[text.Length];

        unknowns[0] = 1;

        for (int i = 1; i < text.Length; i++)
        {
            hashes[i] = ((hashes[i - 1] * unknown) + text[i]) % divider;
            unknowns[i] = unknowns[i - 1] * unknown % divider;
        }

        for (int i = 0; i < count; i++)
        {
            string[] lines = Console.ReadLine().Split(' ');
            bool result = Compare(Convert.ToInt32(lines[0]), Convert.ToInt32(lines[1]) + 1, Convert.ToInt32(lines[2]) + 1);

            stringBuilder = stringBuilder.Append(result ? "yes" : "no");

            if (i < count - 1)
                stringBuilder = stringBuilder.Append('\n');
        }

        Console.WriteLine(stringBuilder);

        bool Compare(int length, int first, int second) => (hashes[first + length - 1] + (hashes[second - 1] * unknowns[length])) % divider
                == (hashes[second + length - 1] + (hashes[first - 1] * unknowns[length])) % divider;
    }

    // Clear comparer (if no caching then will be low speed):

    static bool Compare(string text, int length, int first, int second)
    {
        // Need to cache hashes and unknows start
        text = ' ' + text;

        first++;
        second++;

        int unknown = 257;
        long divider = 1000000007;

        long[] unknowns = new long[text.Length];
        long[] hashes = new long[text.Length];

        unknowns[0] = 1;

        for (int i = 1; i < text.Length; i++)
        {
            hashes[i] = ((hashes[i - 1] * unknown) + text[i]) % divider;
            unknowns[i] = unknowns[i - 1] * unknown % divider;
        }
        // Need to cache hashes and unknows end

        return (hashes[first + length - 1] + (hashes[second - 1] * unknowns[length])) % divider
            == (hashes[second + length - 1] + (hashes[first - 1] * unknowns[length])) % divider;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------

    // uncompleted
    #region Z Preffixes

    private static void GetZPreffixes()
    {
        Console.WriteLine(GetZPreffixes(Console.ReadLine()));

        static string GetZPreffixes(string text)
        {
            int startIndex = -1;
            int size = 0;
            int length = text.Length;

            StringBuilder stringBuilder = new("0 ", length * 2);

            for (int i = 1; i < length; i++)
            {
                if (text[size] == text[i])
                {
                    size++;

                    if (i != length - 1)
                    {
                        if (startIndex == -1)
                            startIndex = i;

                        continue;
                    }
                }

                stringBuilder.Append(size.ToString() + ' ');

                if (startIndex == -1)
                    continue;

                i = startIndex;

                startIndex = -1;
                size = 0;
            }

            return stringBuilder.ToString();
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------

    #region Dijkstra Finding

    private static void GetDistanceBetween()
    {
        string[] lines = Console.ReadLine().Split(' ');

        int vertices = Convert.ToInt32(lines[0]);
        int start = Convert.ToInt32(lines[1]);
        int end = Convert.ToInt32(lines[2]);

        Dictionary<int, List<Path>> paths = new(vertices);

        for (int i = 0; i < vertices; i++)
        {
            lines = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            List<Path> verticePaths = new(vertices - 1);

            for (int j = 0; j < lines.Length; j++)
            {
                if (i == j)
                    continue;

                int weigth = Convert.ToInt32(lines[j]);

                if (weigth == -1)
                    continue;

                verticePaths.Add(new() { Weigth = weigth, End = j + 1 });
            }

            paths.Add(i + 1, verticePaths);
        }

        Dijkstra dijkstra = new(paths);
        Console.WriteLine(dijkstra.GetDistanceBetween(start, end));
    }

    public class Dijkstra
    {
        private readonly Dictionary<int, List<Path>> _paths;
        private readonly Dictionary<int, int> _shortestDistances;
        private readonly HashSet<int> _checkedVertices;
        private readonly HashSet<int> _uncheckedVertices;

        public Dijkstra(Dictionary<int, List<Path>> paths)
        {
            _paths = paths;

            int verticesCount = _paths.Count;

            _shortestDistances = new(verticesCount);
            _checkedVertices = new(verticesCount);
            _uncheckedVertices = new(verticesCount);
        }

        public int GetDistanceBetween(int start, int end)
        {
            _shortestDistances.Add(start, 0);
            _uncheckedVertices.Add(start);

            while (_uncheckedVertices.Count > 0)
            {
                int vertice = _uncheckedVertices.OrderBy((i) => _shortestDistances[i]).First();

                if (vertice == end)
                    break;

                _uncheckedVertices.Remove(vertice);
                _checkedVertices.Add(vertice);

                int baseDistance = _shortestDistances[vertice];

                List<Path> verticePaths = _paths[vertice];

                foreach (Path path in verticePaths)
                {
                    bool shortDistanceSetted = _shortestDistances.TryGetValue(path.End, out int gettedDistance);

                    int alreadySettedDistance = shortDistanceSetted ? gettedDistance : int.MaxValue;
                    int weightDistance = baseDistance + path.Weigth;

                    if (weightDistance >= alreadySettedDistance)
                        continue;

                    if (shortDistanceSetted)
                        _shortestDistances.Remove(path.End);

                    _shortestDistances.Add(path.End, weightDistance);

                    if (_checkedVertices.Contains(path.End) || _uncheckedVertices.Contains(path.End))
                        continue;

                    _uncheckedVertices.Add(path.End);
                }
            }

            return _shortestDistances.TryGetValue(end, out int endDistance) ? endDistance : -1;
        }
    }

    public struct Path
    {
        public int Weigth;
        public int End;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------

    #region Dijkstra Finding with path

    private static void GetDistanceBetweenWithPath()
    {
        string[] lines = Console.ReadLine().Split(' ');

        int vertices = Convert.ToInt32(lines[0]);
        int start = Convert.ToInt32(lines[1]);
        int end = Convert.ToInt32(lines[2]);

        Dictionary<int, List<Path>> paths = new(vertices);

        for (int i = 0; i < vertices; i++)
        {
            lines = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            List<Path> verticePaths = new(vertices - 1);

            for (int j = 0; j < lines.Length; j++)
            {
                if (i == j)
                    continue;

                int weigth = Convert.ToInt32(lines[j]);

                if (weigth == -1)
                    continue;

                verticePaths.Add(new() { Weigth = weigth, End = j + 1 });
            }

            paths.Add(i + 1, verticePaths);
        }

        Dijkstra2 dijkstra = new(paths);
        Console.WriteLine(dijkstra.GetDistanceBetweenWithPath(start, end));
    }

    public class Dijkstra2
    {
        private readonly Dictionary<int, List<Path>> _paths;
        private readonly Dictionary<int, ShortestDistance> _shortestDistances;
        private readonly HashSet<int> _checkedVertices;
        private readonly HashSet<int> _uncheckedVertices;

        public Dijkstra2(Dictionary<int, List<Path>> paths)
        {
            _paths = paths;

            int verticesCount = _paths.Count;

            _shortestDistances = new(verticesCount);
            _checkedVertices = new(verticesCount);
            _uncheckedVertices = new(verticesCount);
        }

        public string GetDistanceBetweenWithPath(int start, int end)
        {
            _shortestDistances.Add(start, new() { Start = -1, Weigth = 0 });
            _uncheckedVertices.Add(start);

            while (_uncheckedVertices.Count > 0)
            {
                int vertice = _uncheckedVertices.OrderBy((i) => _shortestDistances[i].Weigth).First();

                if (vertice == end)
                    break;

                _uncheckedVertices.Remove(vertice);
                _checkedVertices.Add(vertice);

                int baseDistance = _shortestDistances[vertice].Weigth;

                List<Path> verticePaths = _paths[vertice];

                foreach (Path path in verticePaths)
                {
                    bool shortDistanceSetted = _shortestDistances.TryGetValue(path.End, out ShortestDistance gettedDistance);

                    int alreadySettedDistance = shortDistanceSetted ? gettedDistance.Weigth : int.MaxValue;
                    int weightDistance = baseDistance + path.Weigth;

                    if (weightDistance >= alreadySettedDistance)
                        continue;

                    if (shortDistanceSetted)
                        _shortestDistances.Remove(path.End);

                    _shortestDistances.Add(path.End, new() { Start = vertice, Weigth = weightDistance });

                    if (_checkedVertices.Contains(path.End) || _uncheckedVertices.Contains(path.End))
                        continue;

                    _uncheckedVertices.Add(path.End);
                }
            }

            if (!_shortestDistances.TryGetValue(end, out ShortestDistance endDistance))
                return "-1";

            StringBuilder stringBuilder = new(end.ToString() + ' ', 10);

            int currentVertice = end;

            while (currentVertice != start)
            {
                currentVertice = _shortestDistances[currentVertice].Start;
                stringBuilder.Append(currentVertice.ToString() + ' ');
            }

            string[] indexes = stringBuilder.ToString().TrimEnd().Split(' ');
            string text = string.Empty;

            for (int i = 0; i < indexes.Length; i++)
                text += indexes[indexes.Length - 1 - i] + ' ';

            return text;
        }
    }

    public struct ShortestDistance
    {
        public int Weigth;
        public int Start;
    }

    #endregion

    // ----------------------------------------------------------------------------------------------

    // Final test:

    #region Task A

    private static void A()
    {
        long input = Convert.ToInt32(Console.ReadLine());

        long m;
        long dividedBy2;
        long dividedBy3;
        long dividedBy6;

        long result = 1;
        long highInput = input * input;

        while (highInput > result)
        {
            m = (result + highInput) >> 1;

            dividedBy2 = (long)Math.Pow((m + 1.0e-6), 1.0 / 2);
            dividedBy3 = (long)Math.Pow((m + 1.0e-6), 1.0 / 3);
            dividedBy6 = (long)Math.Pow((m + 1.0e-6), 1.0 / 6);

            if (dividedBy2 + dividedBy3 - dividedBy6 < input)
                result = m + 1;
            else
                highInput = m;
        }

        Console.WriteLine(result);
    }

    #endregion

    // ----------------------------------------------------------------------------------------------

    #region Task B

    private static void B()
    {
        string[] lines = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        long requiredSize = Convert.ToInt32(lines[0]);
        int bricksTypesCount = Convert.ToInt32(lines[1]);
        int bricksCount = bricksTypesCount * 2;

        lines = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int[] bricks = new int[bricksCount];

        for (int i = 0; i < bricksCount; i += 2)
        {
            int brickSize = Convert.ToInt32(lines[i / 2]);

            bricks[i] = brickSize;
            bricks[i + 1] = brickSize;
        }

        Console.WriteLine(GetResult());

        string GetResult()
        {
            bool isMoreThenMaxSize = false;

            Dictionary<int, long> successBranch = new(bricksCount);
            Dictionary<int, long> bricksBranch = new(bricksCount); // brickIndex - brickSize

            FindBranch(-1, 0);

            if (successBranch.Count == 0)
                return isMoreThenMaxSize ? "0" : "-1";

            StringBuilder stringBuilder = new(successBranch.Count * 10);
            stringBuilder.Append(successBranch.Count.ToString() + '\n');

            foreach (long brickSize in successBranch.Values)
                stringBuilder.Append(brickSize.ToString() + ' ');

            return stringBuilder.ToString();

            void FindBranch(int currentIndex, long currentSize)
            {
                int brickSize;
                long currentWithBrickSize;

                for (int i = 0; i < bricksCount; i++)
                {
                    if (i == currentIndex || bricksBranch.ContainsKey(i))
                        continue;

                    brickSize = bricks[i];
                    currentWithBrickSize = currentSize + brickSize;

                    if (currentWithBrickSize > requiredSize)
                    {
                        isMoreThenMaxSize = true;
                        continue;
                    }
                    else if (currentWithBrickSize == requiredSize && (successBranch.Count == 0 || bricksBranch.Count + 1 < successBranch.Count))
                    {
                        successBranch.Clear();

                        foreach (KeyValuePair<int, long> brick in bricksBranch)
                            successBranch.Add(brick.Key, brick.Value);

                        successBranch.Add(i, brickSize);

                        break;
                    }

                    if (successBranch.Count != 0 && bricksBranch.Count + 1 >= successBranch.Count)
                        break;

                    bricksBranch.Add(i, brickSize);
                    FindBranch(i, currentWithBrickSize);
                    bricksBranch.Remove(i);
                }
            }
        }
    }

    #endregion

    // ----------------------------------------------------------------------------------------------
}