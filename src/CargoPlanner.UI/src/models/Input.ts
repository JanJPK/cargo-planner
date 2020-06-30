export type Instance = {
  userId: string;
  items: Item[];
  truck: Truck;
};

export type Item = {
  type: number;
  description: string;
  width: number;
  height: number;
  depth: number;
  weight: number;
  count: number;
};

export type Truck = {
  width: number;
  height: number;
  depth: number;
  frontAxle: Axle;
  rearAxle: Axle;
};

export type Axle = {
  offset: number;
  initialLoad: number;
  maximumLoad: number;
};

export function testItems() {
  const testItems: Item[] = [
    {
      type: 1,
      description: 'item desc',
      width: 100,
      height: 100,
      depth: 100,
      weight: 100,
      count: 2,
    },
    {
      type: 2,
      description: '',
      width: 200,
      height: 200,
      depth: 200,
      weight: 200,
      count: 4,
    },
    {
      type: 3,
      description: 'item desc',
      width: 300,
      height: 300,
      depth: 300,
      weight: 300,
      count: 6,
    },
  ];
  return testItems;
}
