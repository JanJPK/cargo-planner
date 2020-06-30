import React, { useState, useRef, useEffect } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { getResult } from '../../services/ResultService';
import { Container, Card, ListGroup, Button, Col, Row } from 'react-bootstrap';
import { Result, Truck, Item, defaultTruck } from '../../models/Result';
import { getInstance, solveInstance } from '../../services/InstanceService';
import ViewResultCanvas from './ViewResultCanvas';

interface RouteParams {
  instanceId: string;
  resultId: string;
  truckIndex: string;
}

function ViewResult({ match, history }: RouteComponentProps<RouteParams>) {
  const [trucks, setTrucks] = useState<Truck[]>([]);
  const instanceId = match.params.instanceId;
  const resultId = match.params.resultId;
  const [truck, setTruck] = useState<Truck>(defaultTruck());
  const [selectedItem, setSelectedItem] = useState<Item>();

  useEffect(() => {
    const fetchResult = async () => {
      if (match.params.resultId === 'solve') {
        const newResultId = await solveInstance(match.params.instanceId);
        history.push('/result/' + instanceId + '/' + newResultId + '/0');
        history.go(0);
      }

      const loadedResult = await getResult(resultId);
      setTruck({
        ...loadedResult.trucks[Number(match.params.truckIndex)],
        items: [...loadedResult.trucks[Number(match.params.truckIndex)].items],
      });
      setTrucks(loadedResult.trucks);
    };

    fetchResult();
  }, []);

  function SelectedItem() {
    if (selectedItem != null) {
      return (
        <Card style={{ margin: 10 }}>
          <Card.Body>
            <Card.Title>Selected item </Card.Title>
            <Card.Text>
              {selectedItem.description === ''
                ? 'No description'
                : selectedItem.description}
            </Card.Text>
          </Card.Body>
          <ListGroup className="list-group-flush">
            <ListGroup.Item>
              {selectedItem.depth}0 x {selectedItem.width}0 x{' '}
              {selectedItem.height}0 cm
            </ListGroup.Item>
            <ListGroup.Item>{selectedItem.weight}0 kg</ListGroup.Item>
          </ListGroup>
        </Card>
      );
    } else {
      return (
        <Card style={{ margin: 10 }}>
          <Card.Body>
            <Card.Title>No item selected </Card.Title>
          </Card.Body>
        </Card>
      );
    }
  }

  return (
    <div
      style={{
        height: '100%',
        position: 'absolute',
        left: '0px',
        width: '100%',
        overflow: 'hidden',
      }}
    >
      <Container fluid>
        <Row>
          <ViewResultCanvas truck={truck} boxClickHandler={setSelectedItem} />
        </Row>
        <Row>
          <Col>
            <ListGroup>
              {trucks.map((truck, i) => {
                return (
                  <Button
                    onClick={() => {
                      history.push(
                        '/result/' +
                          match.params.instanceId +
                          '/' +
                          match.params.resultId +
                          '/' +
                          i,
                      );
                      history.go(0);
                    }}
                  >
                    Truck {i + 1}
                  </Button>
                );
              })}
            </ListGroup>
          </Col>
          <Col>
            <SelectedItem />
          </Col>
        </Row>
      </Container>
    </div>
  );
}

export default ViewResult;
