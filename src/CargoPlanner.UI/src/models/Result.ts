export type Result = {
  trucks: Truck[];
  timeInMilliseconds: number;
};

export type Truck = {
  width: number;
  height: number;
  depth: number;
  items: Item[];
  mesh: Mesh;
};

export type Axle = {
  offset: number;
  maximumLoad: number;
  finalLoad: number;
};

export type Item = {
  type: number;
  width: number;
  height: number;
  depth: number;
  weight: number;
  description: string;
  mesh: Mesh;
};

export type Mesh = {
  x: number;
  y: number;
  z: number;
  color: string;
  opacity: number;
};

export function testItems() {
  const testItems: Item[] = [
    {
      type: 1,
      description: 'item 1',
      width: 3,
      height: 3,
      depth: 3,
      weight: 3,
      mesh: {
        color: '#4287f5',
        x: 1,
        y: 1,
        z: 1,
        opacity: 1.0,
      },
    },
    {
      type: 2,
      description: 'item 2',
      width: 5,
      height: 5,
      depth: 5,
      weight: 3,
      mesh: {
        color: '#4287f5',
        x: 1,
        y: 1,
        z: 1,
        opacity: 1.0,
      },
    },
  ];
  return testItems;
}

export function defaultTruck() {
  const defaultTruck: Truck = {
    depth: 1,
    width: 1,
    height: 1,
    items: [],
    mesh: {
      color: '#424242',
      x: 0,
      y: 0,
      z: 0,
      opacity: 0.3,
    },
  };
  return defaultTruck;
}
