import React from 'react';
import ReactDOM from 'react-dom';
import { Container, Row, Spinner } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

import './index.css';
import { initializeInterop } from './helpers/lspInterop';
import { Playground } from './components/Playground';
import { createLanguageClient } from './helpers/client';

ReactDOM.render(
  <Container className="d-flex vh-100">
    <Row className="m-auto align-self-center">
      <Spinner animation="border" variant="light" />
    </Row>
  </Container>,
  document.getElementById('root')
);

initializeAndCreateClient()
  .then((client) => ReactDOM.render(
    <div className="app-container">
      <Playground client={client} />
    </div>,
    document.getElementById('root')
  ));

async function initializeAndCreateClient() {
  await initializeInterop(self);
  const client = await createLanguageClient();

  return client;
}