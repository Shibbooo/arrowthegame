using System;

namespace Comsole
{
	public class Entity
	{
		public string name;
		
		public int id;
		public int speed;
		public int lifesOnDestroy;
		public int createTime;
		public int lifeTime;

		public staticobjects.types type;
		public staticobjects.directions direction;
		
		protected bool hot = false;
		protected Position position;
		
		protected Entity(){}
		
		public Entity(string name, int id, int x, int y, staticobjects.types type)
		{
			this.name = name;
			this.id = id;
			this.position = new Position(x, y);
			this.type = type;
			this.direction = staticobjects.directions.NONE;
		}
		
		public staticobjects.types getType()
		{
			return this.type;
		}
		
		public void setType(staticobjects.types type)
		{
			this.type = type;
		}
		
		public void setDirection(staticobjects.directions dir)
		{
			this.direction = dir;
		}
		
		public staticobjects.directions getDirection()
		{
			return this.direction;
		}
		
		public void setPosition(int x, int y)
		{
			this.position.update(x, y);
		}
		
		public Position getPosition()
		{
			return this.position;
		}
		
		public int getRow()
		{
			return this.position.y;
		}
		
		public int getCol()
		{
			return this.position.x;
		}
		
		public void setDanger(bool danger)
		{
			this.hot = danger;
		}
		
		public bool IsDangerous()
		{
			return hot;
		}
	}
}
