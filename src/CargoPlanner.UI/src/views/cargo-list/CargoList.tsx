import React, { FC, useState, useEffect, FormEvent } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { getInstances } from '../../services/InstanceService';
import { Instance } from '../../models/ListItem';
import {
  Button,
  ButtonGroup,
  Container,
  Row,
  Col,
  ListGroup,
  Form,
  Card,
} from 'react-bootstrap';
import { getUserId } from '../../services/UserService';

function CargoList({ history }: RouteComponentProps) {
  const userId: string = getUserId();

  const [instances, setInstances] = useState<Instance[]>([]);

  useEffect(() => {
    const fetchInstances = async () => {
      const loadedInstances: Instance[] = await getInstances(userId);
      setInstances(loadedInstances);
    };

    fetchInstances();
  }, []);

  function handleEdit(instanceId: string) {
    history.push('/cargo-edit/' + instanceId);
  }

  function handleSolve(instanceId: string) {
    history.push('/result/' + instanceId + '/solve/0');
  }

  return (
    <ListGroup>
      {instances.map((instance, i) => {
        return (
          <ListGroup.Item key={i}>
            {instance.display + ', ' + instance.itemCount + ' items'}

            <ButtonGroup className="float-right">
              <Button
                variant="secondary"
                onClick={() => handleEdit(instance.id)}
              >
                Edit
              </Button>
              <Button
                variant="primary"
                onClick={() => handleSolve(instance.id)}
              >
                Solve
              </Button>
            </ButtonGroup>
          </ListGroup.Item>
        );
      })}
    </ListGroup>
  );
}

export default CargoList;
