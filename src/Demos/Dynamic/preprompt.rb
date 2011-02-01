class Session
  attr_reader :title, :speakers, :date, :summary

  def initialize(title, speakers, date, topics)
    @title, @speakers, @date, @topics = title, speakers, date, topics
  end

  def to_s
    "Session #{@title}, on #{@date}, by #{speakers}, covering #{@topics}";
  end
end

class SessionDescription
  def initialize(title, &b)
    @title = title
    instance_eval &b
  end

  def session
    @session ||= Session.new(@title, @speakers, @date, @topics)
  end

  def on(date)
    @date = date
  end

  def contains_topics(topics)
    @topics = topics
  end

  def speakers_are(*speakers)
    @speakers = speakers
  end
end

class PrePromptDescription
  attr_reader :sessions

  def initialize(&b)
    @sessions = []
    instance_eval &b
  end

  def add_session(title, &b)
    @sessions << SessionDescription.new(title, &b).session
  end
end

PrePromptDescription.new do
  add_session 'Aspectos internos e idiomas de programacao em C#' do
    on '26/01/2011'
    contains_topics ['LINQ',
                     'Dynamic Typing' ]
    speakers_are 'P. Felix', 'D. Nunes', 'J. Trindade'
  end

  add_session 'Programacao Paralela e Assincrona na Plataforma .NET' do
    on '26/01/2011'
    contains_topics [ 'TPL',
                      'PLINQ' ]
    speakers_are 'C. Martins', 'D. Nunes'
  end  
end