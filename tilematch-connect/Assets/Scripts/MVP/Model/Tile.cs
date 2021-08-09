using UniRx;

public class Tile
{
    public enum TileEnum { normal, bomb }
    public struct TileCoords
    {
        public int X, Y;
        public TileCoords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    public ReactiveProperty<int> TileID;
    public ReactiveProperty<int> ImageID;
    public ReactiveProperty<TileCoords> Coords;
    //public ReactiveProperty<int> X;
    //public ReactiveProperty<int> Y;
    public ReactiveProperty<TileEnum> TileType;
    public BoolReactiveProperty TileLive;
    public BoolReactiveProperty TileGlow;


    public Tile(int tildId, int imageId, TileCoords coords, TileEnum tileType = (int)TileEnum.normal, bool tileLive = true, bool tileGlow = false)
    {
        TileID = new ReactiveProperty<int>(tildId);
        ImageID = new ReactiveProperty<int>(imageId);
        Coords = new ReactiveProperty<TileCoords>(coords);
        TileType = new ReactiveProperty<TileEnum>(tileType);
        TileLive = new BoolReactiveProperty(tileLive);
        TileGlow = new BoolReactiveProperty(tileGlow);
    }
}
