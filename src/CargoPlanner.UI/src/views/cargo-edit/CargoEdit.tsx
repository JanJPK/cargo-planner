import React, { FC, useState, useEffect, FormEvent } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Item, Truck, Axle, Instance } from '../../models/Input';
import ItemListItem from './ItemListItem';
import { Button, Container, Col, ListGroup, Form, Card } from 'react-bootstrap';
import {
  getInstance,
  postInstance,
  putInstance,
  deleteInstance,
} from '../../services/InstanceService';
import { InstancedInterleavedBuffer } from 'three';
import { httpGet } from '../../services/HttpService';
import { getUserId } from '../../services/UserService';

interface RouteParams {
  id: string;
}

function CargoEdit({ history, match }: RouteComponentProps<RouteParams>) {
  const userId: string = getUserId();
  const [instanceId, setInstanceId] = useState('');
  const [items, setItems] = React.useState<Item[]>([]);
  const [item, setItem] = useState<Item>(defaultItem());
  const [truck, setTruck] = useState<Truck>(defaultTruck());

  useEffect(() => {
    const fetchInstance = async () => {
      if (match.params.id !== 'new') {
        setInstanceId(match.params.id);
        const loadedInstance: Instance = await getInstance(match.params.id);
        setItems(loadedInstance.items);
        setTruck(loadedInstance.truck);
      }
    };

    fetchInstance();
  }, []);

  function getNewId() {
    const lastItem = items.sort((a, b) => a.type - b.type)[items.length - 1];
    if (lastItem === undefined) {
      return 1;
    } else {
      return lastItem.type + 1;
    }
  }

  function defaultInstance() {
    const defaultInstance: Instance = {
      userId: userId,
      items: [],
      truck: defaultTruck(),
    };
    return defaultInstance;
  }

  function defaultItem(id: number = 0) {
    const defaultItem: Item = {
      type: id,
      description: '',
      width: 0,
      depth: 0,
      height: 0,
      weight: 0,
      count: 1,
    };
    return defaultItem;
  }

  function defaultTruck() {
    const defaultTruck: Truck = {
      width: 0,
      depth: 0,
      height: 0,
      frontAxle: {
        offset: 0,
        initialLoad: 0,
        maximumLoad: 0,
      },
      rearAxle: {
        offset: 0,
        initialLoad: 0,
        maximumLoad: 0,
      },
    };
    return defaultTruck;
  }
  function handleEdit(i: number) {
    setItem(items[i]);
  }

  function handleDelete(i: number) {
    setItems([...items.slice(0, i), ...items.slice(i + 1)]);
  }

  function handleAdd() {
    console.log(items);
    setItems([...items, { ...item, type: getNewId() }]);
    setItem(defaultItem());
  }

  function handleSave() {
    const i = items.findIndex((x) => x.type === item.type);
    setItems([...items.slice(0, i), item, ...items.slice(i + 1)]);
    setItem(defaultItem());
  }

  function removeItem(i: number) {
    setItems([...items.slice(0, i), ...items.slice(i + 1)]);
  }

  function handleSend() {
    if (instanceId !== '') {
      putInstance(
        {
          userId: userId,
          truck: truck,
          items: items,
        },
        instanceId,
      );
    } else {
      postInstance({
        userId: userId,
        truck: truck,
        items: items,
      });
    }
  }

  function handleSolve() {
    history.push('/result/' + instanceId + '/solve/0');
    history.go(0);
  }

  return (
    <div>
      <style type="text/css">
        {`
        .list-group {
          overflow-x:auto;
        }

        .button {          
          margin: 10px
          padding: 10px
        }
        `}
      </style>
      <Container fluid>
        <br />
        <Card>
          <Card.Body>
            <Card.Title>Truck</Card.Title>
            <Form>
              <Form.Row>
                <Form.Group as={Col}>
                  <Form.Label>Width</Form.Label>
                  <Form.Control
                    value={truck.width}
                    onChange={(e) =>
                      setTruck({ ...truck, width: Number(e.target.value) })
                    }
                    placeholder="Width [cm]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Height</Form.Label>
                  <Form.Control
                    value={truck.height}
                    onChange={(e) =>
                      setTruck({ ...truck, height: Number(e.target.value) })
                    }
                    placeholder="Height [cm]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Depth</Form.Label>
                  <Form.Control
                    value={truck.depth}
                    onChange={(e) =>
                      setTruck({ ...truck, depth: Number(e.target.value) })
                    }
                    placeholder="Depth [cm]"
                  />
                </Form.Group>
              </Form.Row>
            </Form>
          </Card.Body>
          <Card.Body>
            <Card.Subtitle>Front axle</Card.Subtitle>
            <Form>
              <Form.Row>
                <Form.Group as={Col}>
                  <Form.Label>Offset</Form.Label>
                  <Form.Control
                    value={truck.frontAxle.offset}
                    onChange={(e) =>
                      setTruck({
                        ...truck,
                        frontAxle: {
                          ...truck.frontAxle,
                          offset: Number(e.target.value),
                        },
                      })
                    }
                    placeholder="Offset [cm]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Initial load</Form.Label>
                  <Form.Control
                    value={truck.frontAxle.initialLoad}
                    onChange={(e) =>
                      setTruck({
                        ...truck,
                        frontAxle: {
                          ...truck.frontAxle,
                          initialLoad: Number(e.target.value),
                        },
                      })
                    }
                    placeholder="Initial load [kg]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Maximum load</Form.Label>
                  <Form.Control
                    value={truck.frontAxle.maximumLoad}
                    onChange={(e) =>
                      setTruck({
                        ...truck,
                        frontAxle: {
                          ...truck.frontAxle,
                          maximumLoad: Number(e.target.value),
                        },
                      })
                    }
                    placeholder="Maximum load [kg]"
                  />
                </Form.Group>
              </Form.Row>
            </Form>
            <Card.Subtitle>Rear axle</Card.Subtitle>
            <Form>
              <Form.Row>
                <Form.Group as={Col}>
                  <Form.Label>Offset</Form.Label>
                  <Form.Control
                    value={truck.rearAxle.offset}
                    onChange={(e) =>
                      setTruck({
                        ...truck,
                        rearAxle: {
                          ...truck.rearAxle,
                          offset: Number(e.target.value),
                        },
                      })
                    }
                    placeholder="Offset [cm]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Initial load</Form.Label>
                  <Form.Control
                    value={truck.rearAxle.initialLoad}
                    onChange={(e) =>
                      setTruck({
                        ...truck,
                        rearAxle: {
                          ...truck.rearAxle,
                          initialLoad: Number(e.target.value),
                        },
                      })
                    }
                    placeholder="Initial load [kg]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Maximum load</Form.Label>
                  <Form.Control
                    value={truck.rearAxle.maximumLoad}
                    onChange={(e) =>
                      setTruck({
                        ...truck,
                        rearAxle: {
                          ...truck.rearAxle,
                          maximumLoad: Number(e.target.value),
                        },
                      })
                    }
                    placeholder="Maximum load [kg]"
                  />
                </Form.Group>
              </Form.Row>
            </Form>
          </Card.Body>
        </Card>
        <br />
        <Card>
          <Card.Body>
            <Card.Title>Cargo</Card.Title>
            <Form>
              <Form.Row>
                <Form.Group as={Col}>
                  <Form.Label>Description (optional)</Form.Label>
                  <Form.Control
                    value={item.description}
                    onChange={(e) =>
                      setItem({ ...item, description: e.target.value })
                    }
                    type="text"
                    placeholder="Description"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Weight</Form.Label>
                  <Form.Control
                    value={item.weight}
                    onChange={(e) =>
                      setItem({ ...item, weight: Number(e.target.value) })
                    }
                    placeholder="Weight [kg]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Count:</Form.Label>
                  <Form.Control
                    value={item.count}
                    onChange={(e) =>
                      setItem({ ...item, count: Number(e.target.value) })
                    }
                  />
                </Form.Group>
              </Form.Row>
              <Form.Row>
                <Form.Group as={Col}>
                  <Form.Label>Width</Form.Label>
                  <Form.Control
                    value={item.width}
                    onChange={(e) =>
                      setItem({ ...item, width: Number(e.target.value) })
                    }
                    placeholder="Width [cm]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Height</Form.Label>
                  <Form.Control
                    value={item.height}
                    onChange={(e) =>
                      setItem({ ...item, height: Number(e.target.value) })
                    }
                    placeholder="Height [cm]"
                  />
                </Form.Group>
                <Form.Group as={Col}>
                  <Form.Label>Depth</Form.Label>
                  <Form.Control
                    value={item.depth}
                    onChange={(e) =>
                      setItem({ ...item, depth: Number(e.target.value) })
                    }
                    placeholder="Depth [cm]"
                  />
                </Form.Group>
              </Form.Row>
              {item.type === 0 ? (
                <Button onClick={handleAdd}>Add</Button>
              ) : (
                <Button variant="success" onClick={handleSave}>
                  Save
                </Button>
              )}{' '}
              <Button
                onClick={() => {
                  setItem(defaultItem());
                }}
              >
                New
              </Button>
            </Form>
          </Card.Body>
        </Card>
        <br />
        <ListGroup horizontal>
          {items.map((item, i) => {
            return (
              <ListGroup.Item key={i}>
                <ItemListItem item={item} />
                <br />
                <Button
                  size="sm"
                  variant="secondary"
                  onClick={() => handleEdit(i)}
                >
                  Edit
                </Button>{' '}
                <Button
                  size="sm"
                  variant="danger"
                  onClick={() => handleDelete(i)}
                >
                  Delete
                </Button>
              </ListGroup.Item>
            );
          })}
        </ListGroup>
        <br />
        <Button variant="success" onClick={handleSend}>
          Save changes
        </Button>{' '}
        {match.params.id !== 'new' ? (
          <Button onClick={handleSolve}>Solve</Button>
        ) : (
          <Button disabled>Solve</Button>
        )}
      </Container>
    </div>
  );
}

export default CargoEdit;
