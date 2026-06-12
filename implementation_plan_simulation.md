<b>RESUMO:</b>


Com base na documentação do Museu Faraday e na estrutura do backend que analisámos, os 3 cenários de simulação que tens de implementar na Unity são os
  seguintes. Estes cenários representam a evolução histórica da instalação na Chamusca:

  ---

  Cenário A: Alimentação Exclusiva por Baterias (Modo Noturno)
  Este cenário representa o uso da energia acumulada quando o motor está desligado (ex: durante a noite).

   * Objetivo: Manter a iluminação da casa estável enquanto as baterias descarregam.
   * Componentes Ativos:
       * Tudor Battery Bank: 60 células em série.
       * Control Board (Lado da Descarga): Voltímetro de linha, Amperímetro de consumo.
       * Double Selector Switch: O componente crítico. Como a voltagem das células cai com a descarga, o utilizador tem de "rodar" o seletor para incluir
         mais células (das últimas 20) no circuito para manter os 110V-120V.
   * O que liga ao quê: Baterias → Seletor de Células → Reóstato → Carga (Luzes da Casa).
   * Configuração na Unity: Motor Crossley e Motor ASEA em estado OFF. O script ChamuscaAPIClient deve enviar o estado da carga (consumo) e receber a taxa
     de descarga.

  ---

  Cenário B: Geração via Motor Térmico Crossley (c. 1920)
  Este é o cenário original de eletrificação autónoma antes de chegar a rede pública à Chamusca.

   * Objetivo: Ligar o motor a combustão para carregar as baterias e alimentar a casa simultaneamente.
   * Componentes Ativos:
       * Motor Crossley: Motor monocilíndrico a tempos.
       * Correia de Transmissão: Liga o volante do Crossley ao dínamo.
       * Dínamo ASEA DC: Atua como gerador.
       * Control Board (Lado da Carga): Reóstato de campo para ajustar a excitação do dínamo.
   * O que liga ao quê: Crossley (Mecânico) → Dínamo → Quadro de Mármore → Baterias + Casa.
   * Configuração na Unity:
       * Animação da correia e volantes.
       * O utilizador ajusta o Reóstato para garantir que a voltagem do dínamo é superior à das baterias para que a corrente flua no sentido da carga.
       * O script SimpleSwitchTest.cs (ou similar) deve fechar o disjuntor de carga.

  ---

  Cenário C: Integração com a Rede Pública AC (c. 1929)
  Representa a modernização da propriedade. O motor Crossley torna-se redundante para eletricidade e passa a usar-se a rede pública.

   * Objetivo: Usar o motor elétrico trifásico para rodar o dínamo e carregar as baterias.
   * Componentes Ativos:
       * Rede Elétrica Pública (AC): Fornece os 380V/120V.
       * Motor de Indução ASEA (3-Phase): Atua como motor primário.
       * Dínamo ASEA DC: Acoplado mecanicamente ao motor elétrico (atuando como conversor AC/DC).
       * Baterias Tudor.
   * O que liga ao quê: Rede AC → Motor de Indução → Dínamo DC → Baterias.
   * Configuração na Unity:
       * Motor Crossley em estado OFF ou IDLE (desacoplado).
       * O utilizador liga o interruptor trifásico no quadro.
       * A velocidade de rotação é agora constante (ditada pela frequência da rede 50Hz), tornando a simulação da voltagem mais estável.

  ---

  Resumo Técnico para os teus Scripts:

  ┌────────────────┬────────────────────┬─────────────────────────────────┬─────────────────────────┐
  │ Parâmetro      │ Cenário A          │ Cenário B                       │ Cenário C               │
  ├────────────────┼────────────────────┼─────────────────────────────────┼─────────────────────────┤
  │ Fonte Primária │ Baterias (Químico) │ Crossley (Combustão)            │ Rede Pública (Elétrico) │
  │ Conversor      │ N/A                │ Dínamo ASEA (Gerador)           │ Motor ASEA + Dínamo     │
  │ Input Crítico  │ Seletor de Células │ Velocidade do Motor + Reóstato  │ Reóstato de Campo       │
  │ Output Visual  │ Queda de Voltagem  │ Faíscas no Dínamo + Ruído Motor │ Estabilidade de Rotação │
  └────────────────┴────────────────────┴─────────────────────────────────┴─────────────────────────┘

  Como ligar isto à simulação (FMU):
  Na Unity, os teus scripts (como o ChamuscaAPIClient.cs) devem enviar para o backend:
   1. scenario_id (A, B ou C).
   2. rheostat_position (0.0 a 1.0).
   3. selector_position (número da célula de 40 a 60).
   4. load_demand (número de lâmpadas ligadas).

  O backend (OpenModelica) responde com:
   * current_V e current_I (para mexeres os ponteiros dos instrumentos na Unity).
   * battery_SOC (State of Charge, para saberes se as baterias estão a ficar vazias).

---


<b>DETALHES DE IMPLEMENTAÇÃO:</b>


Abaixo encontras o plano detalhado de implementação para os três cenários de simulação na Unity, desenhados para interagir com o backend FastAPI e os
  modelos FMU (OpenModelica).

  Plano de Implementação: Cenários de Simulação Speedma

  Este documento detalha os requisitos técnicos, lógicos e visuais para a implementação dos três estados históricos do aparato de gestão de energia da
  Chamusca.

  ---

  1. Arquitetura Geral da Simulação
  Para todos os cenários, a Unity atua como a interface de I/O (Input/Output).
   * Inputs (Enviados para o Backend): Posição de interruptores (boolean), posição do reóstato (0.0-1.0), posição do seletor de células (índice 40-60) e
     carga aplicada (resistência ou kW).
   * Outputs (Recebidos do FMU): Voltagem da linha (V), Corrente (I), Velocidade angular (ω) e Estado de Carga da Bateria (SOC).

  ---

  Cenário A: Descarga e Regulação (Modo Noturno)
  Contexto Histórico: O período entre sessões de carga onde a casa é alimentada exclusivamente pela energia química armazenada nas baterias Tudor.

  A.1 Componentes Ativos
   * Tudor Battery Bank: 60 células de chumbo-ácido.
   * Marble Control Board (Lado de Descarga):
       * Interruptor Geral de Saída: Liga/Desliga a alimentação da casa.
       * Double Selector Switch: Seletor rotativo ligado às últimas 20 células da bateria.
       * Voltímetro de Saída: Mostra a tensão na linha.
       * Amperímetro de Consumo: Mostra a corrente exigida pelas lâmpadas.

  A.2 Lógica e Conexões
   * Circuito: Bateria (Série) -> Seletor de Células -> Disjuntor -> Carga.
   * O Desafio do Utilizador: À medida que as baterias descarregam, a voltagem cai abaixo dos 110V. O utilizador deve rodar o seletor para incluir mais
     células em série no circuito (os "taps" de compensação) para manter a iluminação estável.
   * Falha: Se o utilizador não adicionar células, as luzes piscam ou apagam. Se adicionar a mais, a voltagem excessiva "queima" as lâmpadas (feedback
     visual).

  A.3 Implementação Unity
   * Scripts: CellSelector.cs (captura o índice da célula e envia via API).
   * Visual: Intensidade das luzes da sala (Light.intensity) ligada diretamente ao valor de V recebido do backend.

  ---

  Cenário B: Geração Térmica Isolada (c. 1920)
  Contexto Histórico: O sistema original. O motor a combustão Crossley é a única fonte de energia, convertendo combustível em rotação mecânica para o
  dínamo.

  B.1 Componentes Ativos
   * Motor Crossley: Motor monocilíndrico horizontal.
   * Correia de Couro (Flat Belt): Transmissão mecânica por fricção.
   * Dínamo ASEA DC: Atuando como gerador de corrente contínua.
   * Marble Control Board (Lado de Carga):
       * Reóstato de Excitação: Controla a intensidade do campo magnético do dínamo.
       * Amperímetro de Carga: Mostra a corrente a entrar nas baterias.

  B.2 Lógica e Conexões
   * Circuito: Motor Crossley (Mecânico) -> Dínamo (Elétrico) -> Reóstato -> Baterias.
   * O Desafio do Utilizador:
       1. Colocar o motor Crossley em funcionamento (RPM nominal).
       2. Ajustar o Reóstato de campo. Se a voltagem gerada pelo dínamo for inferior à das baterias, a corrente flui no sentido contrário (o dínamo tenta
          atuar como motor), o que é um risco técnico.
       3. Equilibrar a carga: O utilizador deve garantir que gera energia suficiente para carregar as baterias e alimentar a casa simultaneamente.

  B.3 Implementação Unity
   * Scripts: BeltAnimation.cs (a velocidade da textura da correia e a rotação dos volantes devem ser proporcionais ao valor de ω recebido do backend).
   * VFX: Fumo no escape do Crossley (partículas) e faíscas nas escovas do dínamo se a carga for excessiva.

  ---

  Cenário C: Modernização e Integração AC (c. 1929)
  Contexto Histórico: A chegada da rede pública à Chamusca. O motor elétrico ASEA substitui o motor Crossley na função de rodar o dínamo (conversão AC/DC).

  C.1 Componentes Ativos
   * Rede Elétrica Pública (Mains): Entrada trifásica AC.
   * Motor de Indução ASEA: Motor trifásico acoplado ao dínamo.
   * Dínamo ASEA DC: Atua como gerador, rodado pelo motor elétrico.
   * Control Board (Painel AC):
       * Interruptor de Faca (Knife Switch): Liga o motor à rede AC.

  C.2 Lógica e Conexões
   * Circuito: Rede AC -> Motor ASEA (Mecânico) -> Dínamo DC -> Baterias.
   * O Desafio do Utilizador:
       1. Ligar a alimentação AC. O motor arranca e estabiliza numa rotação fixa (sincronizada com a frequência da rede, 50Hz).
       2. A regulação é agora muito mais estável do que com o motor térmico, mas o utilizador ainda deve gerir o reóstato para controlar a taxa de carga das
          baterias.
       3. Este cenário permite simular o custo da energia da rede vs. a autonomia das baterias.

  C.3 Implementação Unity
   * Scripts: ACSwitch.cs (liga a alimentação trifásica).
   * Áudio: Substituir o som rítmico do Crossley por um zumbido constante de 50Hz do motor elétrico e o som de alta frequência do dínamo.
   * Interação: O motor Crossley deve aparecer parado ou desconectado (correia removida ou movida para a polia louca).

  ---

  Resumo de Comandos API (Payloads Sugeridos)

  ┌─────────┬───────────┬───────────────────────────────────────────────────────┐
  │ Cenário │ Endpoint  │ Exemplo de Payload JSON                               │
  ├─────────┼───────────┼───────────────────────────────────────────────────────┤
  │ A       │ /sim/step │ {"scenario": "A", "load": 5.0, "cell_index": 52}      │
  │ B       │ /sim/step │ {"scenario": "B", "engine_rpm": 350, "rheostat": 0.7} │
  │ C       │ /sim/step │ {"scenario": "C", "ac_mains": true, "rheostat": 0.4}  │
  └─────────┴───────────┴───────────────────────────────────────────────────────┘

  Nota sobre as FMUs: Certifica-te de que os nomes das variáveis no OpenModelica (ex: R_load, field_resistance, n_cells) coincidem com as chaves que a Unity
  envia no script ChamuscaAPIClient.cs.

---
<b>AINDA MAIS DETALHES DE IMPLEMENTAÇÃO:</b>

![alt text](20260213_152612.jpg)

Esta imagem é um tesouro técnico e permite-nos detalhar os cenários com uma precisão absoluta, usando os nomes originais e a lógica de cablagem exata
  desenhada em 1920.

  Aqui está a atualização do plano de simulação, agora com os detalhes extraídos diretamente do esquema do "Reductor duplo" e do "Quadro de distribuição".

  ---

  Especificação Técnica de Simulação: Aparato Chamusca 1920

  Este detalhamento técnico deve servir de base para a criação dos scripts na Unity e para a lógica do servidor de simulação.

  1. O Painel de Controlo (Mármore)
  Baseado na imagem, o utilizador terá de interagir com os seguintes elementos no painel:

   * Instrumentos de Medição (Topo):
       * A (Do dínamo): Amperímetro que mede a corrente gerada pelo motor Crossley/ASEA.
       * V (Central): Voltímetro geral da linha.
       * A (Da bateria): Amperímetro que mede o fluxo de carga/descarga das baterias Tudor.
   * Seccionamento e Proteção (Meio):
       * Fuzíveis: Proteções para "Casa", "Bateria" e "Dependências".
       * Dijunctor: Disjuntor principal da Bateria (proteção contra sobrecarga).
   * Comutação de Iluminação:
       * Interruptores de faca para "Iluminação da casa" e "Iluminação depend." (Dependências/Anexos).
       * Interruptores menores para "Luz no quadro" e "Luz na bateria".
   * Controlo de Carga:
       * Interruptores de faca para "Dínamo LUZ" e "Bateria LUZ".
       * Comutador central de "Carga bateria".

  ---

  2. Detalhamento dos Cenários

  Cenário A: Descarga e Regulação (Modo Noturno)
   * Workflow: O motor está parado. A energia vem da "Bateria de acumuladores".
   * Interação Crítica: O utilizador deve usar o "Reductor duplo" no lado da "Descarga".
   * Lógica Física: À medida que as 60 células perdem carga, a voltagem cai. O esquema mostra que o Redutor está ligado às células 41 a 60. O utilizador
     deve rodar o manípulo para "meter" mais células no circuito e manter o Voltímetro nos 110V-120V.
   * Visual na Unity: O Amperímetro "Da bateria" mostra um valor negativo (corrente a sair). Se o utilizador não rodar o redutor, o Voltímetro central desce
     e as luzes da casa perdem brilho.

  Cenário B: Carga via Motor Crossley (c. 1920)
   * Workflow: O Motor Crossley está a rodar o Dínamo.
   * Interação Crítica:
       1. Ajustar o "Rheostato" (à esquerda do quadro) para controlar a excitação do dínamo.
       2. Monitorizar o amperímetro "Do dínamo".
       3. Usar o "Reductor duplo" no lado da "Carga" para selecionar a voltagem ideal de entrada nas baterias.
   * Lógica Física: O objetivo é garantir que a voltagem do Dínamo seja ligeiramente superior à da Bateria para que ocorra a carga.
   * Risco: Se o "Dijunctor" saltar, o sistema corta a ligação para evitar danos nas células Tudor.

  Cenário C: Carga via Rede Pública e Motor ASEA (c. 1929)
   * Workflow: O motor elétrico ASEA está ligado à rede AC e roda o Dínamo DC.
   * Interação Crítica: Ligar o interruptor de faca do motor trifásico (que não está neste esquema mas aparece nas fotos do museu como um painel
     secundário).
   * Regulação: Uma vez o motor em RPM nominal, o utilizador volta ao quadro principal para gerir a "Carga bateria" e o "Reductor duplo".
   * Vantagem: A rotação é constante, pelo que a agulha do amperímetro "Do dínamo" é muito mais estável do que no Cenário B.

  ---

  3. Mapeamento de Ficheiros e Variáveis para a Unity

  ┌─────────────────┬───────────────────────────────┬────────────────────────────────────────────┐
  │ Nome no Esquema │ Variável no Script (#my_code) │ Função Visual na Unity                     │
  ├─────────────────┼───────────────────────────────┼────────────────────────────────────────────┤
  │ Rheostato       │ float rheostatPos             │ Rotação do manípulo de excitação.          │
  │ Reductor duplo  │ int cellTapIndex              │ Rotação do seletor circular (20 posições). │
  │ A Do dínamo     │ float dynamoAmps              │ Rotação do ponteiro (0-50A).               │
  │ A Da bateria    │ float batteryAmps             │ Rotação do ponteiro (-30A a +30A).         │
  │ V               │ float lineVolts               │ Rotação do ponteiro (0-200V).              │
  │ Iluminação casa │ bool houseLightSwitch         │ Toggle do interruptor de faca esquerdo.    │
  └─────────────────┴───────────────────────────────┴────────────────────────────────────────────┘
  ---

  4. Notas para o Servidor FMU (OpenModelica)
  O teu modelo no OpenModelica deve contemplar:
   1. Modelo da Bateria: 60 células com curva de descarga real.
   2. Modelo do Redutor: Uma resistência variável ou um comutador de fontes que adiciona 2.2V (voltagem nominal de uma célula) por cada passo do seletor.
   3. Lógica de Carga/Descarga: O redutor tem dois braços (Carga e Descarga) para permitir que se carregue a bateria a uma voltagem enquanto se fornece
      outra à casa (isto é visível nas linhas duplas que saem do redutor para os contactos das células).

  Este esquema é o "mapa" que o júri mais vai valorizar se o vires replicado funcionalmente na Unity!