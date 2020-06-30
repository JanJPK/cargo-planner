import React from 'react';
import { Jumbotron, Container, Row, Col } from 'react-bootstrap';

const NotFound = () => (
  <Jumbotron fluid>
    <Container>
      <Row className="justify-content-md-center">
        <Col md="auto">
          <h1>404 Not Found</h1>
        </Col>
      </Row>
    </Container>
  </Jumbotron>
);

export default NotFound;
