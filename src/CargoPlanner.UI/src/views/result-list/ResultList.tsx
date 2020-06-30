import React, { FC, useState, useEffect } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Result } from '../../models/ListItem';
import { getResults } from '../../services/ResultService';
import {
  Button,
  Container,
  Row,
  Col,
  ListGroup,
  ButtonGroup,
} from 'react-bootstrap';
import { getUserId } from '../../services/UserService';

function ResultList({ history }: RouteComponentProps) {
  const userId: string = getUserId();

  const [results, setResults] = useState<Result[]>([]);

  useEffect(() => {
    const fetchInstances = async () => {
      const loadedResults: Result[] = await getResults(userId);
      setResults(loadedResults);
    };

    fetchInstances();
  }, []);

  function handleView(instanceId: string, resultId: string) {
    history.push('/result/' + instanceId + '/' + resultId + '/0');
  }

  return (
    <ListGroup>
      {results.map((result, i) => {
        return (
          <ListGroup.Item key={i}>
            {result.display + ', Instance [' + result.instanceId + ']'}

            <ButtonGroup className="float-right">
              <Button
                variant="primary"
                onClick={() => handleView(result.instanceId, result.id)}
              >
                View result
              </Button>
            </ButtonGroup>
          </ListGroup.Item>
        );
      })}
    </ListGroup>
  );
}

export default ResultList;
