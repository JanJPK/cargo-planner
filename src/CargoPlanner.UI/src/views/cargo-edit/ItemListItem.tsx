import React, { FC } from 'react';
import { Card, ListGroup } from 'react-bootstrap';
import { Item } from '../../models/Input';

interface Props {
  item: Item;
}

const ItemListItem: FC<Props> = ({ item }) => {
  return (
    <div>
      <style type="text/css">
        {`
        .card {
          min-width: 200px;
        }
        `}
      </style>
      <Card>
        <Card.Body>
          <Card.Title>
            {item.count} x Item #{item.type}
          </Card.Title>
          <Card.Text>
            {item.description === '' ? 'No description' : item.description}
          </Card.Text>
        </Card.Body>
        <ListGroup className="list-group-flush">
          <ListGroup.Item>
            {item.depth} x {item.width} x {item.height} cm
          </ListGroup.Item>
          <ListGroup.Item>{item.weight} kg</ListGroup.Item>
        </ListGroup>
      </Card>
    </div>
  );
};

export default ItemListItem;
