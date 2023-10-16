import time
import argparse
from peaceful_pie.unity_comms import UnityComms

from card import Card
from deck import Deck
from Evaluator import Evaluator



class Player:
  def __init__(self, name):
    self.name = name
    self.cards = []
    self.turn = None
    self.chips = 0
    self.stake = 0
    self.stake_gap = 0
    self.score = []
    self.fold = False
    self.ready = False
    self.all_in = False
    self.list_of_special_attributes = []
    self.card_class = ""
    self.winrate = 0
    self.turn = 0

  def __repr__(self):
     name = self.name
     return name
   

class Pokergame:
  def __init__(self):
    self.deck = Deck()
    self.need_raise_info = False
    self.game_over = False
    self.acting_player = Player('acting_player')
    self.possible_responses = []
    self.past_action = []
    self.round_counter = 0
    self.cards = []
    self.pot = 0
    self.pot_dict = {}
    self.pot_in_play = 0
    self.list_of_player_names = []
    self.dealer = Player('dealer')
    self.small_blind = Player('small_blind')
    self.big_blind = Player('big_blind')
    self.first_actor = Player('first_actor')
    self.winners = []
    self.deck = Deck()
    self.list_of_scores_from_eligible_winners = []
    self.starting_chips = 0
    self.ready_list = []
    self.agent_cards = []
    self.agent_hands = []
    # self.number_of_players = int(input('Enter number of players: '))
    # assert 1 < self.number_of_players < 11, "Invalid number of players"
    # for i in range(self.number_of_players):
    #   x = input('Enter player ' + str(i + 1) + ' name: ')
    #   assert x != "", 'Invalid name'
    self.list_of_player_names = ['Player', 'BOT', 'Agent']
    self.list_of_players = [Player(name) for name in self.list_of_player_names if name != ""]
    for player in self.list_of_players:
      if player.name == 'Agent':
        player.turn = 1
      else:
        player.turn = 0
    self.starting_chips = 100
    assert self.starting_chips > 0, "Invalid number, try greater than 0"
    self.small_blind_amount = self.starting_chips*0.01
    self.big_blind_amount = self.starting_chips*0.02
    for player in self.list_of_players:
      player.chips = self.starting_chips
    self.winner = None
    self.action_counter = 0
    self.highest_stake = 0
    self.fold_list = []
    self.round_ended = False
    self.fold_out = False
    self.list_of_scores_eligible = []
    self.list_of_players_not_out = list(set(self.list_of_players))
    self.number_of_player_not_out = int(len(list(set(self.list_of_players))))
    

  def print_round_info(self):
    print("\n")
    for player in self.list_of_players:
       print("\n")
       print(f"Name: {player.name}")
       print(f"Cards: {Card.ints_to_pretty_str(player.cards)}")
       print(f"Player score: {player.score}")
       print(f"Chips: {player.chips}")
       print(f"Special Attributes: {player.list_of_special_attributes}")
       if player.fold:
        print(f"Folded")
       if player.all_in:
        print(f"All-in")
       print(f"Stake: {player.stake}")
       print(f"Stake-gap: {player.stake_gap}")
       print(f"Card class: {player.card_class}")
       print(f"Winrate: {player.winrate}%")
       print("\n")
    print(f"Pot: {self.pot}")
    print(f"Community cards: {Card.ints_to_pretty_str(self.cards)}")
    print("\n")

  def establish_player_attributes(self):
    address_assignment = 0
    self.dealer = self.list_of_players_not_out[address_assignment]
    address_assignment += 1
    address_assignment %= len(self.list_of_players_not_out)
    self.small_blind = self.list_of_players_not_out[address_assignment]
    self.small_blind.list_of_special_attributes.append("small blind")
    
    address_assignment += 1
    address_assignment %= len(self.list_of_players_not_out)
    self.big_blind = self.list_of_players_not_out[address_assignment]
    self.big_blind.list_of_special_attributes.append("big blind")
    address_assignment += 1
    address_assignment %= len(self.list_of_players_not_out)
    self.first_actor = self.list_of_players_not_out[address_assignment]
    self.first_actor.list_of_special_attributes.append("first actor")
    self.list_of_players_not_out.append(self.list_of_players_not_out.pop(0))

  def to_index(card_in) -> int:
    ranks = '23456789TJQKA'
    suits = '♣♦♥♠'
    dict_rank = {card:val + 1 for val,card in enumerate(ranks)}
    dict_suit = {suit:val + 1 for val,suit in enumerate(suits)}
    rank_str = Card.int_to_pretty_str(card_in)[0]
    suit_str = Card.int_to_pretty_str(card_in)[1]
    card_str = rank_str + suit_str
    ranks = dict_rank[rank_str]
    suits = dict_suit[suit_str]
    list_cards = []
    for i in ranks:
      for j in suits:
        list_cards.append(i + j)
    dict_cards = {card:val +1 for val,card in enumerate(list_cards)}
    carvec = dict_cards[card_str]
    return carvec

  def run(self,args: argparse.Namespace) -> None:
    unity_comms = UnityComms(port=args.port)
    unity_comms.GetCommand(key = args.key)
    
  def deal_hole(self):
    for player in self.list_of_players_not_out:
      player.cards = self.deck.draw(2)
      send_card = argparse.ArgumentParser()
      send_card.add_argument(self.to_index(Card.int_to_pretty_str(player.cards)), type=str, required=True)
      send_card.add_argument('--port', type=int, default=9000)
      # send_name = argparse.ArgumentParser()
      # send_name.add_argument(player.name, type=str, required=True)
      # send_name.add_argument('--port', type=int, default=9000)
      args1 = send_card.parse_args()
      self.run(args1)
      # args2 = send_name.parse_args()
      # self.run(args2)


  def deal_flop(self):
    self.cards = self.deck.draw(3)

  def deal_turn(self):
    self.cards.append(self.deck.draw(1))

  def deal_river(self):
    self.cards.append(self.deck.draw(1))

  def clear_board(self):
    self.possible_responses.clear()
    self.cards.clear()
    self.deck = Deck()
    self.pot = 0
    self.pot_dict.clear()
    self.winners.clear()
    self.list_of_scores_from_eligible_winners.clear()
    self.action_counter = 0
    self.highest_stake = 0
    self.fold_list.clear()
    self.fold_out = False
    self.list_of_scores_eligible.clear()
    self.round_ended = False
    self.past_action = []
    self.turns = []
    self.agent_cards = []
    self.agent_hands = []
    for player in self.list_of_players:
      player.score.clear()
      player.cards.clear()
      player.stake = 0
      player.stake_gap = 0
      player.ready = False
      player.all_in = False
      player.fold = False
      player.list_of_special_attributes.clear()
      player.win = False
      player.card_class = ""
      player.winrate = 0
    self.list_of_players_not_out = list(set(self.list_of_players))
    for player in self.list_of_players_not_out:
      if player.chips <= 0:
        self.list_of_players_not_out.pop(self.list_of_players_not_out.index(player))

  def end_round(self):
    for player in self.list_of_players_not_out:
      if player.chips <= 0 or player.fold:
        self.list_of_players_not_out.pop(self.list_of_players_not_out.index(player))
    self.list_of_players_not_out = list(set(self.list_of_players_not_out))
    for player in self.list_of_players_not_out:
      if player.chips <= 0:
        self.list_of_players_not_out.remove(player)
        print(f"{player.name} is out of the game!")
    self.number_of_player_not_out = len(set(self.list_of_players_not_out))
    if self.number_of_player_not_out == 1:
      self.game_over = True
      self.winner = self.list_of_players_not_out[0]
      self.winner.chips += self.pot if self.winner.chips != 0 else 0
      print(f"Game is over: {self.winner} wins with {self.winner.chips}!" )
      quit()
    self.winners[0].chips += self.pot
    new_round = ""
    new_round = input("Start new round? (yes/no)")
    if new_round == "yes":
      print("\n\n\t\t\t\t--ROUND OVER--")
      print("\n\n\t\t\t--STARTING NEW ROUND--\n")
      self.round_counter += 1
      pass
    else:
     quit()
    time.sleep(0.3)
    self.clear_board()
    
  def run2(self,args: argparse.Namespace) -> None:
    unity_comms = UnityComms(port=args.port)
    res = unity_comms.SendCommand()
    print("res", res)
    
  def get_action(self):
    parser = argparse.ArgumentParser()
    parser.add_argument('--port', type=int, default = 9000)
    args = parser.parse_args()
    self.run2(args)
  
  def answer(self, player):
    self.past_turn(player.turn)
    player.stake_gap = self.highest_stake - player.stake
    if player.all_in or player.fold or self.fold_out:
      return True
    if player.chips <= 0:
      print(f"{player.name} is all in!")
      player.all_in = True
    line = "=" * 10
    pn = player.name + "'s turn"
    print("{} {} {}".format(line,pn,line))
    print(f"Highest stake: {self.highest_stake}")
    print(f"Put in at least {player.stake_gap} to stay in.\nDon't Have that much? You'll have to go all-in!")
    print(f"Chips available: {player.chips}")
    self.possible_responses.clear()
    if player.stake_gap > 0:
      self.possible_responses.append("fold")
    if player.stake_gap == player.chips:
      self.possible_responses.append("all_in")
    if player.stake_gap > player.chips:
      self.possible_responses.append("all_in")
    if player.stake_gap < player.chips:
      self.possible_responses.append("call")
      self.possible_responses.append("all_in")
      self.possible_responses.append("raise")
    if player.stake_gap == 0:
      self.possible_responses.append("check")
      self.possible_responses.append("bet")
      self.possible_responses.append("fold")
      self.possible_responses.append("all_in")
    line = "=" * 10
    pn = player.name + "'s turn"
    if player.turn == 1:
      self.agent_turn(self.possible_responses,player.winrate)
    while True:
      print("{} {} {}".format(line,pn,line))
      print(self.possible_responses)
      response = self.get_action()
      if response not in self.possible_responses:
        print("Invalid response")
        continue
      if response == "all_in":
        self.get_past("all_in")
        print(f"{player.name} is all-in!")
        player.stake += player.chips
        self.pot += player.chips
        player.stake_gap -= player.chips
        player.chips = 0
        player.all_in = True
        if player.stake_gap > player.chips:
          return True
        if player.stake_gap == player.chips:
          player.stake += player.stake_gap
          self.pot += player.stake_gap
          player.stake_gap = 0
        if player.stake_gap == 0:
          player.stake_gap = 0
          self.highest_stake = player.stake
          self.ready_list.clear()
        if player.stake_gap < player.chips:
          player.stake_gap = 0
          player.stake += player.chips
          self.pot += player.chips
          player.chips = 0
          self.highest_stake = player.stake
          self.ready_list.clear()
        return True
      if response == "fold":
        self.get_past("fold")
        player.fold = True
        # self.list_of_players_not_out.pop(self.list_of_players_not_out.index(player))
        self.fold_list.append(player)
        if len(self.fold_list) == (len(self.list_of_players_not_out) - 1):
          for player in self.list_of_players_not_out:
            if player not in self.fold_list:
              self.fold_out = True
              print(f"{player} wins!")
              self.winners.append(player)
              for player in self.winners:
                player.win = True
                self.round_ended = True
        return True
      if response == "call":
        self.get_past("call")
        player.stake += player.stake_gap
        self.pot += player.stake_gap
        player.chips -= player.stake_gap
        player.stake_gap = 0
        return True
      if response == "check":
        self.get_past("check")
        player.stake_gap = 0
        return True
      if response == "bet" or "raise":
        if response == " bet":
          self.get_past("bet")
        if response == " raise":
          self.get_past("raise")
        self.need_raise_info = True
        bet = int(input(f"How much would {player.name} like to raise? ({player.chips} available)\n->"))
        if bet > player.chips or bet <= 0:
          print("Invalid response")
        if player.stake_gap == 0:
          player.stake += bet
          self.pot += bet
          player.chips -= bet
          self.highest_stake = player.stake
          self.ready_list.clear()
          player.stake_gap = 0
          if bet == player.chips:
            print(f"{player.name} is all-in!")
            player.all_in = True
            return True
          return True
        if player.stake_gap < player.chips:
          player.stake += player.stake_gap
          self.pot += player.stake_gap
          player.chips -= player.stake_gap
          player.stake_gap = 0
          player.stake += bet
          self.pot += bet
          player.chips -= bet
          self.highest_stake = player.stake
          self.ready_list.clear()
          player.stake_gap = 0
          if bet == player.chips:
            print(f"{player.name} is all-in!")
            player.all_in = True
          return True
        return True
      print("Invalid Response")

  def ask_players(self):
    self.ready_list.clear()
    starting_index = self.list_of_players_not_out.index(self.first_actor)
    for player in self.list_of_players_not_out:
      player.ready = False
    while True:
      self.acting_player = self.list_of_players_not_out[starting_index]
      player_ready = self.answer(self.list_of_players_not_out[starting_index])
      starting_index += 1
      starting_index %= len(self.list_of_players_not_out)
      if player_ready:
        self.ready_list.append("gogo")
      if len(self.ready_list) == len(self.list_of_players_not_out):
        break

  def act_one(self):
    if self.small_blind_amount > self.small_blind.chips:
      self.small_blind.stake += self.small_blind.chips
      self.highest_stake = self.small_blind.chips
      self.pot += self.small_blind.chips
      self.small_blind.chips = 0
      print(f"{self.small_blind.name} is all-in!")
      self.small_blind.all_in = True
    else:
     self.small_blind.chips -= self.small_blind_amount
     self.small_blind.stake += self.small_blind_amount
     self.highest_stake = self.small_blind_amount
     self.pot += self.small_blind_amount
    if self.big_blind_amount > self.big_blind.chips:
     self.big_blind.stake += self.big_blind.chips
     self.highest_stake = self.big_blind.chips
     self.pot += self.big_blind.chips
     self.big_blind.chips = 0
     print(f"{self.big_blind} is all-in!")
     self.big_blind.all_in = True
    else:
     self.big_blind.chips -= self.big_blind_amount
     self.big_blind.stake += self.big_blind_amount
     self.highest_stake = self.big_blind_amount
     self.pot += self.big_blind_amount
     self.ask_players()
     
  def get_past(self, act : str) -> None:
    self.past_action.append(act)
    if len(self.past_action) > 30:
      self.past_action.pop(0)
    
  def past_turn(self, turn : int):
    self.turns.append(turn)
    
  def get_agent_cards(self, cards: list[int]):
    for card in cards:
      index = self.to_index(Card.int_to_pretty_str(card))
      self.agent_cards.append(index)
      self.agent_hands = [self.agent_cards[0],self.agent_cards[1]]
    assert 2 <= len(cards) <= 7, "Invalid input can't get agent cards"
    for i in range(7 - len(cards)):
      self.agent_card(0)
      
  def agent_turn(self, avc : list[str] , wr : int):
    
    return avc, self.pot, self.past_action, self.turns, wr, self.agent_hands, self.agent_cards

  def play(self):
    EV = Evaluator()
    self.establish_player_attributes()
    self.deal_hole()
    self.get_agent_cards(Card.int_to_pretty_str(self.list_of_players[2].cards))
    self.print_round_info()
    self.act_one()
    self.print_round_info()
    if not self.round_ended:
      self.deal_flop()
      self.get_agent_cards(Card.int_to_pretty_str(self.cards))
      EV.flop_evaluate(self.cards,self.list_of_players_not_out)
      self.print_round_info()
    if not self.round_ended:
      self.ask_players()
      self.print_round_info()
    if not self.round_ended:
      self.deal_turn()
      self.get_agent_cards(Card.int_to_pretty_str(self.cards[3]))
      EV.flop_evaluate(self.cards,self.list_of_players_not_out)
      self.print_round_info()
    if not self.round_ended:
      self.ask_players()
      self.print_round_info()
    if not self.round_ended:
      
      self.deal_river()
      self.get_agent_cards(Card.int_to_pretty_str(self.cards[4]))
      EV.flop_evaluate(self.cards,self.list_of_players_not_out)
      self.print_round_info()
    if not self.round_ended:
      self.ask_players()
      self.print_round_info()
    if not self.round_ended:
      self.print_round_info()

    self.winners = EV.hand_summary(self.cards,self.list_of_players_not_out)
    self.round_ended = True
    self.end_round()

  def run_game_data(self):
    while True:
      self.play()