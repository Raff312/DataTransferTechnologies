namespace Lab3.FaultTolerance;

public class ByteGraph(int rows, int cols) {
    private readonly byte[,] _matrix = new byte[rows, cols];

    public ByteGraph(List<byte> values, int cols) : this(values.Count / cols, cols) {
        for (var i = 0; i < Rows; i++) {
            for (var j = 0; j < Cols; j++) {
                _matrix[i, j] = values[i * Cols + j];
            }
        }
    }

    public byte this[int i, int j] {
        get => _matrix[i, j];
        set => _matrix[i, j] = value;
    }

    public int Rows => _matrix.GetLength(0);
    public int Cols => _matrix.GetLength(1);

    public bool IsSymmetric() {
        if (Rows != Cols) {
            return false;
        }

        for (var i = 0; i < Rows; i++) {
            for (var j = 0; j < i; j++) {
                if (_matrix[i, j] != _matrix[j, i]) {
                    return false;
                }
            }
        }

        return true;
    }

    public int GetConnectivity() {
        var possibleConnectivity = GetMinConnections();

        for (var i = possibleConnectivity; i > 0; i--) {
            if (HasConnectivityOf(i)) {
                return i;
            }
        }

        return 0;
    }

    private int GetMinConnections() {
        var minConnections = Rows;

        for (var i = 0; i < Rows; i++) {
            var vertexConnections = 0;

            for (var j = 0; j < Cols; j++) {
                if (_matrix[i, j] > 0) {
                    vertexConnections++;
                }
            }

            minConnections = Math.Min(minConnections, vertexConnections);
        }

        return minConnections;
    }

    private bool HasConnectivityOf(int value) {
        for (var i = 0; i < value; i++) {
            for (var j = i + 1; j < Rows; j++) {
                var connections = GetUniquePathsCount(i, j);
                if (connections < value - i) {
                    return false;
                }
            }

            for (var j = 0; j < Rows; j++) {
                _matrix[i, j] = 0;
                _matrix[j, i] = 0;
            }
        }

        return true;
    }

    public int GetUniquePathsCount(int from, int to) {
        var stack = new Stack<List<int>>();
        stack.Push([from]);

        var paths = new List<List<int>>();

        while (stack.Count > 0) {
            var path = stack.Pop();
            var current = path[^1];

            if (current == to) {
                paths.Add(new List<int>(path));
                continue;
            }

            for (var i = 0; i < Rows; i++) {
                if (_matrix[current, i] > 0 && !path.Contains(i)) {
                    var newPath = new List<int>(path) { i };
                    stack.Push(newPath);
                }
            }
        }

        var uniquePaths = new HashSet<int>();
        var counter = 0;
        foreach (var path in paths.OrderBy(x => x.Count)) {
            var isUnique = true;
            for (var i = 1; i < path.Count - 1; i++) {
                if (uniquePaths.Contains(path[i])) {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique) {
                for (var i = 1; i < path.Count - 1; i++) {
                    uniquePaths.Add(path[i]);
                }
                counter++;
            }
        }

        return counter;
    }
}
